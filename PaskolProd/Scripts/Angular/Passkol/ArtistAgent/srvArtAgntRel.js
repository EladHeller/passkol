angular.module("passkolApp")
    .factory("srvArtAgntRel", ["$http", function ( $http) {
        return {
            AgntRemovedByArtAgnt: function (data, v) {
                return $http({
                    method: 'POST',
                    url: '/ArtistAgent/AgntRemovedByArtAgnt',
                    data: data,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            AgntRemovedByArt: function (data, v) {
                return $http({
                    method: 'POST',
                    url: '/ArtistAgent/AgntRemovedByArt',
                    data: data,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            AgntConfirmedByArtAgnt: function (data, v) {
                return $http({
                    method: 'POST',
                    url: '/ArtistAgent/AgntConfirmedByArtAgnt',
                    data: data,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            AgntConfirmedByArt: function (data, v) {
                return $http({
                    method: 'POST',
                    url: '/ArtistAgent/AgntConfirmedByArt',
                    data: data,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            AgntConfirmedViaEmailByArtAgnt: function (data, v) {
                return $http({
                    method: 'POST',
                    url: '/ArtistAgent/AgntConfirmedViaEmailByArtAgnt',
                    data: data,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
             AgntConfirmedViaEmailByArt: function (data, v) {
                return $http({
                    method: 'POST',
                    url: '/ArtistAgent/AgntConfirmedViaEmailByArt',
                    data: data,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            }
        }
    }]);