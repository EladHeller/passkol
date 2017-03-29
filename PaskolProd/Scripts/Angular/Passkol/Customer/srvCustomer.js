angular.module("passkolApp")
    .factory("srvCustomer", ["$http", function ( $http) {
        return {
            updateProfile: function (EditedCustomer, v) {
                return $http({
                    method: 'POST',
                    url: '/Account/updateCusomerProfile',
                    data: EditedCustomer,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            }
        }
    }]);