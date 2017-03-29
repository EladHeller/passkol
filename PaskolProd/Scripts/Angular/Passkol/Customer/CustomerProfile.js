angular.module('passkolApp')
    .controller('CustomerProfile', ['$scope', 'srvCustomer','$timeout', function ($scope, srvCustomer,$timeout) {
        
        function saveOriginalVal() {
            $scope.OriginalUserName = angular.copy($scope.UserName);
            $scope.OriginalEmail = angular.copy($scope.email);
            $scope.OriginalPhoneNumber = angular.copy($scope.PhoneNumber);
            $scope.OriginalCompanyName = angular.copy($scope.CompanyName);
            $scope.OriginalPassword = angular.copy($scope.Password);
        }

        function getSelectedData() {
            return modelData = {
                Name: $scope.UserName,
                Email: $scope.email,
                Phone: $scope.PhoneNumber,
                CompanyName: $scope.CompanyName,
                Password: $scope.Password
            };
        }

        $scope.updateName = function () {
            prop = 'Name';
            var req = getSelectedData();
            req['property'] = prop;
            srvCustomer.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
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

        $scope.updateEmail = function () {
            prop = 'Email';
            var req = getSelectedData();
            req['property'] = prop;
            srvCustomer.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
                if (!res.data.Succeeded) {
                    $scope[prop + 'Invalid'] = true;
                    $scope[prop + 'InvalidMsg'] = res.data.FiledErrors['Email'][0];
                }
                else {
                    $scope[prop + 'Invalid'] = false;
                    $scope.focus = null;
                    $scope.OriginalEmail = angular.copy($scope.email);
                }
            });
        }

        $scope.updatePhoneNumber = function () {
            prop = 'Phone';
            var req = getSelectedData();
            req['property'] = prop;
            srvCustomer.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
                if (!res.data.Succeeded) {
                    $scope[prop + 'Invalid'] = true;
                    $scope[prop + 'InvalidMsg'] = res.data.FiledErrors['Phone'][0];
                }
                else {
                    $scope[prop + 'Invalid'] = false;
                    $scope.focus = null;
                    $scope.OriginalPhoneNumber = angular.copy($scope.PhoneNumber);
                }
            });
        }

        $scope.updateCompanyName = function () {
            prop = 'CompanyName';
            var req = getSelectedData();
            req['property'] = prop;
            srvCustomer.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
                if (!res.data.Succeeded) {
                    $scope[prop + 'Invalid'] = true;
                    $scope[prop + 'InvalidMsg'] = res.data.errors[0];
                }
                else {
                    $scope[prop + 'Invalid'] = false;
                    $scope.focus = null;
                    $scope.OriginalCompanyName = angular.copy($scope.CompanyName);
                }
            });
        }

        $scope.updatePassword = function () {
            prop = 'Password';
            var req = getSelectedData();
            req['property'] = prop;
            srvCustomer.updateProfile(req, $scope.antiForgeryToken).then(function (res) {
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

        $timeout(saveOriginalVal);
    }]);