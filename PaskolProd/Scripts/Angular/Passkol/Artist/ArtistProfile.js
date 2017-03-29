angular.module('passkolApp')
    .controller('ArtistProfile', ['$scope', 'srvArtist', 'srvAuction', '$timeout',
        function ($scope, srvArtist, srvAuction, $timeout) {
            $scope.uploader = {};
            //uploadFile = function(){
            //    var file = document.getElementById('f_UploadImage').files[0];
            //    var formData = new FormData();
            //    formData.append("upload", file);
            //    formData.append("property", "Photo");
            //    ajax = new XMLHttpRequest();
            //    ajax.upload.addEventListener("progress", progressHandler, false);
            //    ajax.addEventListener("load", completeHandler, false);
            //    ajax.open("POST", "/Account/updateArtistProfile");
            //    ajax.send(formData);
            //}

            //function progressHandler(event){
            //    var percent = (event.loaded / event.total) * 100;
            //    $('.progress-bar').css({'width':percent + '%'}); //from bootstrap bar class
            //}

            //function completeHandler(){
            //    $('.progress-bar').css({ 'width': '100%' });
            //    $timeout(function () {
            //        window.location.reload();
            //    },500);
                
            //}

        $scope.href.path = 'MyProfile';
       
        function saveOriginalVal() {
            $scope.OriginalUserName = angular.copy($scope.UserName);
            $scope.OriginalEmail = angular.copy($scope.Email);
            $scope.OriginalPageUrl = angular.copy($scope.PageUrl);
            $scope.OriginalContactManName = angular.copy($scope.ContactManName);
            $scope.OriginalContactManPhone = angular.copy($scope.ContactManPhone);
            $scope.OriginalPassword = angular.copy($scope.Password);
            $scope.OriginalBiography = angular.copy($scope.Biography);
        }

        // Participate with original music toggele
        $scope.participateInAuctionToggele = function (p) {
            var v = $scope.antiForgeryToken;
            srvAuction.participateInAuctionToggele(v, { p: p });
        }

        function getSelectedData() {
            return modelData = {
                Name: $scope.UserName,
                Email: $scope.Email
            };
        }

        $scope.updateName = function () {
            prop = 'Name';
            var req = getSelectedData();
            req['property'] = prop;
            srvArtist.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
                if (!res.data.Succeeded) {
                    $scope[prop + 'Invalid'] = true;
                    $scope[prop + 'InvalidMsg'] = res.data.FiledErrors['Email'][0];
                }
                else {
                    $scope[prop + 'Invalid'] = false;
                    $scope.focus = null;
                    $scope.OriginalUserName = angular.copy($scope.UserName);
                }
            });
        }

        $scope.updatePageUrl = function () {
            prop = 'PublicPage';
            var req = getSelectedData();
            req['property'] = prop;
            req.PublicPage = $scope.PageUrl;
            srvArtist.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
                if (!res.data.Succeeded) {
                    $scope[prop + 'Invalid'] = true;
                    $scope[prop + 'InvalidMsg'] = res.data.FiledErrors['PublicPage'][0];
                }
                else {
                    $scope[prop + 'Invalid'] = false;
                    $scope.focus = null;
                    $scope.OriginalPageUrl = angular.copy($scope.PageUrl);
                }
            });
        }

        $scope.updateEmail = function () {
            prop = 'Email';
            var req = getSelectedData();
            req['property'] = prop;
            srvArtist.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
                if (!res.data.Succeeded) {
                    $scope[prop + 'Invalid'] = true;
                    $scope[prop + 'InvalidMsg'] = res.data.FiledErrors['Email'][0];
                }
                else {
                    $scope[prop + 'Invalid'] = false;
                    $scope.focus = null;
                    $scope.OriginalEmail = angular.copy($scope.Email);
                }
            });
        }

        $scope.updateContactManName = function () {
            prop = 'ContactManName';
            var req = getSelectedData();
            req['property'] = prop;
            req.ContactManName = $scope.ContactManName;
            srvArtist.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
                if (!res.data.Succeeded) {
                    $scope[prop + 'Invalid'] = true;
                    $scope[prop + 'InvalidMsg'] = res.data.FiledErrors[prop][0];
                }
                else {
                    $scope[prop + 'Invalid'] = false;
                    $scope.focus = null;
                    $scope.OriginalContactManName = angular.copy($scope.ContactManName);
                }
            });
        }

        $scope.updateContactManPhone = function () {
            prop = 'ContactManPhone';
            var req = getSelectedData();
            req['property'] = prop;
            req.ContactManPhone = $scope.ContactManPhone;
            srvArtist.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
                if (!res.data.Succeeded) {
                    $scope[prop + 'Invalid'] = true;
                    $scope[prop + 'InvalidMsg'] = res.data.FiledErrors[prop][0];
                }
                else {
                    $scope[prop + 'Invalid'] = false;
                    $scope.focus = null;
                    
                    $scope.OriginalContactManPhone = angular.copy($scope.ContactManPhone);
                   
                }
            });
        }

        $scope.updatePassword = function () {
            prop = 'Password';
            var req = getSelectedData();
            req.Password = $scope.Password;
            req['property'] = prop;
            srvArtist.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
                if (!res.data.Succeeded) {
                    $scope[prop + 'Invalid'] = true;
                    $scope[prop + 'InvalidMsg'] = res.data.FiledErrors['Password'][0];
                }
                else {
                    $scope[prop + 'Invalid'] = false;
                    $scope.focus = null;
                   
                    $scope.OriginalPassword = angular.copy($scope.Password);
                }
            });
        }

        $scope.updateBiography = function () {
            prop = 'Biography';
            var req = getSelectedData();
            req['property'] = prop;
            req.Biography = $scope.Biography;
            srvArtist.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
                if (!res.data.Succeeded) {
                    $scope[prop + 'Invalid'] = true;
                    $scope[prop + 'InvalidMsg'] = res.data.FiledErrors[prop][0];
                }
                else {
                    $scope[prop + 'Invalid'] = false;
                    $scope.focus = null;
                    $scope.OriginalBiography = angular.copy($scope.Biography);
                }
            });
        }

        $scope.updatePhoto = function () {
            prop = 'Photo';
            var req = getSelectedData();
            req['property'] = prop;
            var formData = new FormData();
            formData.append('file', $('#f_UploadImage')[0].files[0]);
            req.upload = $('#f_UploadImage')[0].files[0];
            srvArtist.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
                if (!res.data.Succeeded) {
                    $scope[prop + 'Invalid'] = true;
                    $scope[prop + 'InvalidMsg'] = res.data.FiledErrors[prop][0];
                }
                else {
                    $scope[prop + 'Invalid'] = false;
                    $scope.focus = null;
                }
            });
        }

        $timeout(saveOriginalVal);
    }]);