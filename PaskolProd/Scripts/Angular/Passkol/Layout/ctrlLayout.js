﻿angular.module("passkolApp")
    .controller("ctrlLayout", ["$scope", "UserTypeEnum", "srvLayout", function ($scope, UserTypeEnum, srvLayout) {


        //remove the fuc.. autogenerated element of BotCapthach
        setTimeout(function(){$('.LBD_CaptchaDiv').next().remove()},0);

        // user details
        $scope.userTypes = UserTypeEnum;
        $scope.user = {};
        $scope.register = {};
        $scope.recoverPassword = {};
        $scope.login = {};
        $scope.register.Type = UserTypeEnum.Artist;

        // check if need to open login modal
        if (window.location.search.indexOf('lf') > -1 && window.location.hash.indexOf('lff') == -1) {
            window.location.hash = 'lff';
            $('#LoginModal').modal('show');
        }
        
        // functions
        $scope.setReferrer = function () {
            $scope.referrer = document.referrer === "" ? document.URL : document.referrer;
        }

        // register customer
        $scope.register.registerCustomer = function () {
            $scope.isLoading = true;

            // get client-side Captcha object instance
            var captchaObj = $("#CustomerCaptchaCode").get(0).Captcha;

            // gather data required for Captcha validation
            $scope.register.customer.CaptchaId = captchaObj.Id;
            $scope.register.customer.InstanceId = captchaObj.InstanceId;
            $scope.register.customer.CaptchaInput = $("#CustomerCaptchaCode").val();
            $scope.register.customer.v = $scope.antiForgeryToken;
            srvLayout.registerCustomer($scope.register.customer).success(function (res) {
                // Check if there are not any errors
                if (res.suceeded) {
                    window.location.href = "/Customer/MyProfile";
                } else {
                    // Handle errors
                    var modelState = res.errors;
                    for (i in modelState) {
                        if (modelState[i].Key == "Name") {
                            $scope.register.registerCustomer.isNameNotValid = true;
                            $scope.register.registerCustomer.NameError = modelState[i].Value;
                        }
                        else if (modelState[i].Key == "Email") {
                            $scope.register.registerCustomer.isEmailNotValid = true;
                            $scope.register.registerCustomer.EmailError = modelState[i].Value;
                        }
                        else if (modelState[i].Key == "Captcha") {
                            $scope.register.registerCustomer.isCaptchaNotValid = true;
                        }
                    }
                }
                
            })
            .error(function (res) {
                var recs = res;
            })
            .finally(function () {
                captchaObj.ReloadImage();
                $scope.isLoading = false;
            });
        }

        // register artist agent
        $scope.register.registerArtistAgent = function () {
            $scope.isLoading = true;

            // get client-side Captcha object instance
            var captchaObj = $("#ArtistAgentCaptchaCode").get(0).Captcha;

            // gather data required for Captcha validation
            $scope.register.artistAgent.CaptchaId = captchaObj.Id;
            $scope.register.artistAgent.InstanceId = captchaObj.InstanceId;
            $scope.register.artistAgent.CaptchaInput = $("#ArtistAgentCaptchaCode").val();
            $scope.register.artistAgent.v = $scope.antiForgeryToken;
            srvLayout.registerArtistAgent($scope.register.artistAgent).success(function (res) {
                // Check if there are not any errors
                if (res.suceeded) {
                    window.location.href = "/ArtistAgent/MyProfile";
                } else {
                    // Handle errors
                    var modelState = res.errors;
                    for (i in modelState) {
                        if (modelState[i].Key == "Name") {
                            $scope.register.registerArtistAgent.isNameNotValid = true;
                            $scope.register.registerArtistAgent.NameError = modelState[i].Value;
                        }
                        else if (modelState[i].Key == "Email") {
                            $scope.register.registerArtistAgent.isEmailNotValid = true;
                            $scope.register.registerArtistAgent.EmailError = modelState[i].Value;
                        }
                        else if (modelState[i].Key == "Captcha") {
                            $scope.register.registerArtistAgent.isCaptchaNotValid = true;
                        }
                    }
                }

            })
            .error(function (res) {
                var recs = res;
            })
            .finally(function () {
                captchaObj.ReloadImage();
                $scope.isLoading = false;
            });
        }

        // regiser artist
        $scope.register.registerArtist = function () {
            $scope.isLoading = true;

            // get client-side Captcha object instance
            var captchaObj = $("#ArtistCaptchaCode").get(0).Captcha;

            // gather data required for Captcha validation
            $scope.register.artist.CaptchaId = captchaObj.Id;
            $scope.register.artist.InstanceId = captchaObj.InstanceId;
            $scope.register.artist.CaptchaInput = $("#ArtistCaptchaCode").val();
            $scope.register.artist.v = $scope.antiForgeryToken;

            srvLayout.registerArtist($scope.register.artist).success(function (res) {
                // Check if there are not any errors
                if (res.suceeded) {
                    window.location.href = "/Artist/MyProfile?isFirstEntrance=1";
                } else {
                    // Handle errors
                    var modelState = res.errors;
                    for (i in modelState) {
                        if (modelState[i].Key == "Name") {
                            $scope.register.registerArtist.isNameNotValid = true;
                            $scope.register.registerArtist.NameError = modelState[i].Value;
                        }
                        else if (modelState[i].Key == "Email") {
                            $scope.register.registerArtist.isEmailNotValid = true;
                            $scope.register.registerArtist.EmailError = modelState[i].Value;
                        }
                        else if (modelState[i].Key == "Captcha") {
                            $scope.register.registerArtist.isCaptchaNotValid = true;
                        }
                    }
                }

            })
            .error(function (res) {
                var recs = res;
            })
            .finally(function () {
                captchaObj.ReloadImage();
                $scope.isLoading = false;
            });
        }

        // submit recover password
        $scope.recoverPassword.submit = function () {
            $scope.isLoading = true;
            var v = $scope.antiForgeryToken;
            srvLayout.recoverPassword($scope.recoverPassword.data, v)
                .then(function (res) {

                })
                .finally(function (res) {
                    $scope.isLoading = false;
                    angular.element('#PasswordRecover').modal('hide');
                    angular.element('#PasswordRecoverConfirm').modal('show');
            })
        }

        // submit set new password
        $scope.recoverPassword.setNewPassword = function () {
            $scope.isLoading = true;
            var v = $scope.antiForgeryToken;
            srvLayout.setNewPassword($scope.recoverPassword.data, v)
                .success(function (res) {
                    if (res.suceeded) {
                        window.location.reload();
                    }
                    else if (res.RecoverFailed) {
                        $scope.recoverPassword.cantRecover = true;
                    }
                    else {
                        $scope.recoverPassword.data.isCodeNotValid = true;
                    }
                    
                })
                .error(function () {
                    $scope.recoverPassword.data.isCodeNotValid = true;
                })
                .finally(function (res) {
                    $scope.isLoading = false;
            })
        }

        // login
        $scope.login.submit = function () {
            $scope.isLoading = true;
            var v = $scope.antiForgeryToken;
            srvLayout.login($scope.login.data, v)
                .success(function (res) {
                    if (res.success) {
                        window.location.reload();
                    }
                    else {
                        $scope.login.isNotSuccedded = true;
                    }
                })
                .error(function (res) {
                    $scope.login.isNotSuccedded = true;
                })
                .finally(function (res) {
                    $scope.isLoading = false;
                })
        }

        // Impersonating
        $scope.register.impersonate = {};
        $scope.register.impersonate.as = 'other';
        $scope.register.impersonate.impersonate = function () {
            var target = $scope.register.impersonate.as == 'other' ? 'impersonating' : 'revertImpersonating';
            var v = $scope.antiForgeryToken;
            srvLayout[target]({ userName: $('#impersonateUserName').val() }, v)
                .success(function (res) {
                    window.location.href = "/";
                });
        }

        
    }]);