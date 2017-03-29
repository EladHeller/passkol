angular.module("passkolApp")
    .controller("ctrlAuction", ["$scope", "srvAuction", "$interval", "$filter",
        function ($scope, srvAuction, $interval, $filter) {
            "use strict"
            var cancelPromise
            $scope.newAuction = {};
            $scope.openAudio = false;
            $scope.currentTime = new Date();
            // Open new
            function GetNotEmptyDate(date) {
                return date == '//' ? null : date;
            }

            function CheckLogicDate() {

                function getDateOrNull(dateString) {
                    return dateString != undefined ? new Date(dateString) : undefined;
                }
                var isValid = true;
                var Today = new Date();
                var OpenProjectDate = getDateOrNull($scope.newAuction.OpenProjectDate);
                var CloseProjectDate = getDateOrNull($scope.newAuction.CloseProjectDate);
                var CloseDate = getDateOrNull($scope.newAuction.CloseDate);
                var PickWinnerDate = getDateOrNull($scope.newAuction.PickWinnerDate);

                if (OpenProjectDate < Today) {
                    $scope.newAuction.OpenProjectDateErr = true;
                    isValid = false;
                } else {
                    $scope.newAuction.OpenProjectDateErr = false;
                }

                if (CloseProjectDate < Today) {
                    $scope.newAuction.CloseProjectDateErr = true;
                    isValid = false;
                } else {
                    $scope.newAuction.CloseProjectDateErr = false;
                }

                if (CloseDate < Today) {
                    $scope.newAuction.CloseDateErr = true;
                    isValid = false;
                } else {
                    $scope.newAuction.CloseDateErr = false;
                }

                if (PickWinnerDate < Today) {
                    $scope.newAuction.PickWinnerDateErr = true;
                    isValid = false;
                } else {
                    $scope.newAuction.PickWinnerDateErr = false;
                }

                return isValid;
            }

            $scope.openNewAuction = function () {
                
                $scope.isLoading = true;
                $scope.newAuction.OpenProjectDate = GetNotEmptyDate($('[name="OpenProjectDate"]').val());
                $scope.newAuction.CloseProjectDate = GetNotEmptyDate($('[name="CloseProjectDate"]').val());
                $scope.newAuction.CloseDate = GetNotEmptyDate($('[name="CloseDate"]').val());
                $scope.newAuction.PickWinnerDate = GetNotEmptyDate($('[name="PickWinnerDate"]').val());

                if (CheckLogicDate()) {
                    var v = $scope.antiForgeryToken;
                    srvAuction.openNewAuction($scope.newAuction, v)
                        .success(function (res) {
                            if (res.Succeeded) {
                                $scope.newAuctoinSucceeded = true;
                                setTimeout(function () {
                                    location.reload();
                                }, 5000);
                            }
                        })
                        .error(function () {

                        })
                        .finally(function (res) {
                            $scope.isLoading = false;
                    });
                }
            }

            // Get all auctions for user
            $scope.getSketchesForAuction = function (id, CloseDate, pickTime, OpenProjectdate) {
                function ddMMyyyToYyyMMdd(date,splitor) {
                    var arr = date.split(splitor);
                    return arr[2] + splitor + arr[1] + splitor + arr[0];
                }

                $scope.isLoading = true;
                $scope.allSketches = [];
                var v = $scope.antiForgeryToken;
                srvAuction.getSketchesForAuction(v, { ID: id })
                    .success(function (res) {
                        if (res.Success) {
                            $scope.sketchMode = true;
                            $scope.allSketches = res.Entities;
                            // Add one day because date has no time(but only date) so the default when you have no time is midnight
                            // the meaning is not including the end of the date(and this is not good) so we added one day and then we included
                            //the end if the end day.
                            $scope.endTime = new Date(ddMMyyyToYyyMMdd(CloseDate, '/')).addDays(1);
                            $scope.pickTime = new Date(ddMMyyyToYyyMMdd(pickTime, '/')).addDays(1);
                            $scope.openProjectDate = new Date(ddMMyyyToYyyMMdd(OpenProjectdate, '/')).addDays(1);

                            
                            if (res.Entities.length > 0) {
                                canBuyFn(res.Entities[0].AuctionId)
                            }
                            
                            cancelPromise = $interval(function () {
                                $scope.currentTime = new Date();
                                //$scope.$apply();
                            }, 1000)

                        }
                    })
                    .error(function (res) {
                        var recs = res;
                    })
                    .finally(function (res) {
                        $scope.isLoading = false;
                    })
            }

            // Get all auctions for user
            $scope.getAuctionsForUser = function () {
                $scope.isLoading = true;
                $scope.allAuctions = [];
                var v = $scope.antiForgeryToken;
                srvAuction.getAuctionsForUser(v)
                    .success(function (res) {
                        if (res.Success) {
                            $scope.allAuctions = res.Entities;
                        }
                    })
                    .error(function (res) {
                        var recs = res;
                    })
                    .finally(function (res) {
                        $scope.isLoading = false;
                    })
            }
            setTimeout(function () {
                $scope.getAuctionsForUser();
            }, 0);

            function canBuyFn(auctionId) {
                var v = $scope.antiForgeryToken;
                srvAuction.canBuy(v, { ID: auctionId })
                   .success(function (res) {
                       $scope.canBuy = res.toLowerCase() === 'true';
                   })
            }

            // Get all auctions for user
            $scope.buySketch = function (auctionId, sketchId) {
                $scope.isLoading = true;
                var v = $scope.antiForgeryToken;
                srvAuction.buySketch(v, { AuctionID: auctionId, SketchID: sketchId })
                    .success(function (res) {
                        if (res.toLowerCase() === 'true') {
                            $('#BuySketchModal').modal('show');
                            $scope.sketchMode = false;
                        }
                    })
                    .error(function (res) {
                        var recs = res;
                    })
                    .finally(function (res) {
                        $scope.isLoading = false;
                });
            }

            $scope.$on('$destroy', function () {
                // Make sure that the interval is destroyed too
                $interval.cancel(cancelPromise);
            });
    }]);