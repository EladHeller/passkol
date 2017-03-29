angular.module("passkolApp")
    .factory("srvAuction", ["$http", function ( $http) {
        return {
            openNewAuction: function (req, v) {
                return $http({
                    method: 'POST',
                    url: '/Auction/OpenNew',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            getAuctionsForUser: function (v) {
                return $http({
                    method: 'POST',
                    url: '/Auction/GetAuctionsForUser',
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            getSketchesForAuction: function (v, req) {
                return $http({
                    method: 'POST',
                    url: '/Auction/GetSketchesForAuction',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            getAuctionsForArtist: function (v, req) {
                return $http({
                    method: 'POST',
                    url: '/Auction/GetAuctionsForArtist',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            participateInAuctionToggele: function (v, req) {
                return $http({
                    method: 'POST',
                    url: '/Auction/ParticipateInAuctionToggele',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            sendSketch: function (v, req) {
                return $http({
                    method: 'POST',
                    url: '/Auction/SendSketch',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            getAuction: function (v, req) {
                return $http({
                    method: 'POST',
                    url: '/Auction/GetAuction',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            uploadSketch: function (v, req) {
                return $http({
                    method: 'POST',
                    url: '/Auction/GetAuction',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            canBuy: function (v, req) {
                return $http({
                    method: 'POST',
                    url: '/Auction/CanBuy',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            },
            buySketch: function (v, req) {
                return $http({
                    method: 'POST',
                    url: '/Auction/BuySketch',
                    data: req,
                    headers: {
                        'RequestVerificationToken': v,
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });
            }
        }
    }]);