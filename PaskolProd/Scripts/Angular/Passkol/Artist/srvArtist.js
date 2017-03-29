angular.module("passkolApp")
    .factory("srvArtist", ["$http", function ( $http) {
        return {
            updateProfile: function (EditedCustomer, v) {
                return $http({
                    method: 'POST',
                    url: '/Account/updateArtistProfile',
                    data: EditedCustomer,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            updateProfilePic: function (EditedCustomer, v) {
                return $http({
                    method: 'POST',
                    url: '/Account/updateArtistProfile',
                    
                    data: EditedCustomer,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            }
        }
    }]);