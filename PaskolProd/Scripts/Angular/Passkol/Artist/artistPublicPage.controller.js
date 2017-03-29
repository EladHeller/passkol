(function () {
    'use strict';

    angular
        .module('passkolApp')
        .controller('artistPublicPageCtrl', artistPublicPageCtrl);

    artistPublicPageCtrl.$inject = ['$scope', 'musicService', 'appModals']; 

    function artistPublicPageCtrl($scope, musicService, appModals) {
        // Data
        var artistId;

        // Methods
        $scope.search = search;
        $scope.executeSearch = executeSearch;

        // Initialize
        musicResultInit();
        
        // Methods Implementation
        function executeSearch(artId) {
            artistId = artId;
            search();
        }

        function search() {
            var data = { Id: artistId, SortType: $scope.sort.sortType, page: $scope.pager.page };
            musicService.musicsByArtistId(data).
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
            $scope.sort = { sortType: '2' };
            $scope.results = [];
            $scope.pager = { page: 1 };
            $scope.TotalResults = 0;
            $scope.TotalPages = 0;
            $scope.sortOptions = [{ text: 'אורך (מהקצר לארוך)', value: '2' },
                                { text: 'אורך (מהארוך לקצר)', value: '3' }];
            $scope.searchLoad = false;
        }
    }
})();
