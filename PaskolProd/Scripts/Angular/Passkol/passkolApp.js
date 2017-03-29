angular.module("passkolApp", ['ngRoute','ngSanitize','ui.bootstrap'])
    .constant('UserTypeEnum', {
        Artist: 'Artist',
        ArtistMgr: 'ArtistMgr',
        Customer: 'Customer'
    });