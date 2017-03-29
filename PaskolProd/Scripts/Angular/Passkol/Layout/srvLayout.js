angular.module("passkolApp")
    .factory("srvLayout", ["$http", function ( $http) {
        return {
            registerCustomer: function(req){
                return $http({
                    method: 'POST',
                    url: '/Account/RegisterCustomer',
                    data: req,
                    headers: {
                        'RequestVerificationToken': req.v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            registerArtist: function (req) {
                return $http({
                    method: 'POST',
                    url: '/Account/RegisterArtist',
                    data: req,
                    headers: {
                        'RequestVerificationToken': req.v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            registerArtistAgent : function(req){
                return $http({
                    method: 'POST',
                    url: '/Account/RegisterArtistAgent',
                    data: req,
                    headers: {
                        'RequestVerificationToken': req.v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            recoverPassword: function (req, v) {
                return $http({
                    method: 'POST',
                    url: '/Account/ForgotPassword',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            setNewPassword: function (req, v) {
                return $http({
                    method: 'POST',
                    url: '/Account/ResetPassword',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            login: function (req, v) {
                return $http({
                    method: 'POST',
                    url: '/Account/Login',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            impersonating: function (req, v) {
                return $http({
                    method: 'POST',
                    url: '/Account/ImpersonateUserAsync',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            revertImpersonating: function (req, v) {
                return $http({
                    method: 'POST',
                    url: '/Account/RevertImpersonationAsync',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            }
        }
    }]);