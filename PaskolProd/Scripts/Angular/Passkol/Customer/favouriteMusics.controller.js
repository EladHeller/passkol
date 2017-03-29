(function () {
    'use strict';

    angular
        .module('passkolApp')
        .controller('userFavouriteMusicsCtrl', UserFavouriteMusicsCtrl);

    UserFavouriteMusicsCtrl.$inject = ['$scope', 'musicService', 'appModals'];

    function UserFavouriteMusicsCtrl($scope, musicService, appModals) {
        // Data
        var customerId;

        // Methods
        $scope.search = search;

        // Initialize
        musicResultInit();
        search();

        // Methods Implementation
        function search() {
            var data = { page: $scope.pager.page };
            musicService.musicsByFavouriteMusic(data).
                then(searchSuccess, searchFailed);
        }

        function searchSuccess(res) {
            $scope.results = res.data.Musics;
            $scope.results.forEach(function (music) {
                music.isNew = music.Tags && !!music.Tags.find(function (tg) {
                    return tg.Level === 1 && tg.Name === 'חדש במערכת';
                });
            });
            $scope.TotalResults = res.data.TotalResults;
            $scope.TotalPages = res.data.TotalPages;
            $scope.pages = new Array($scope.TotalPages);

            for (var i = 0; i < $scope.pages.length; i++) {
                $scope.pages[i] = i;
            }

            $scope.searchLoad = true;
        }
        function searchFailed(res) {
            appModals.alertModal('פסקול', 'אירעה שגיאה בחיפוש');
        }

        function musicResultInit() {
            $scope.resultsFilter = {IsFavourite:true};
            $scope.results = [];
            $scope.pager = { page: 1 };
            $scope.TotalResults = 0;
            $scope.TotalPages = 0;
            $scope.searchLoad = false;
        }
    }
})();
