﻿angular.module('passkolApp')
    .controller('PurchaseReport', ['$scope', 'purchaseService', function ($scope, purchaseService) {

        // init
        $scope.results = [];
        $scope.showOptions = false;
        $scope.sortOptions = [{ text: 'תאריך (מחדש לישן)', value: 0 },
             { text: 'תאריך (מישן לחדש)', value: 1 }];
        $scope.selectedOption = $scope.sortOptions[0];

        // load reports
        $scope.getReports = function () {
            var req = { sort: $scope.selectedOption.value, Index: 1 };
            purchaseService.getCustomerReports(req, $scope.antiForgeryToken).then(function (res) {
                $scope.results = res.data.entities
            });
        }

        // handle select sorting
        $scope.optionSelect = function (option) {
            $scope.selectedOption = option;
            $scope.getReports();
        }

        // handle print reports
        $scope.printReports = function () {
            window.print();
        }

        $scope.getReports();
    }]);