(function () {
    'use strict';
    angular.module('passkolApp')
        .controller('artistSongsCtrl', artistSongsCtrl);
    artistSongsCtrl.$inject = ['$scope', 'musicService', 'searchService', 'appModals', '$timeout'];
    function artistSongsCtrl($scope, musicService, searchService, appModals, $timeout) {
        // Data
        var tagDictionary = {};

        //Methods
        $scope.uploadNewMusic = uploadNewMusic;
        $scope.search = search;
        $scope.editMusic = editMusic;
        // Init
        musicEditInit();
        musicResultsInit();
        search();
        load();

        // Methods implement
        function load() {
            if (location.search) {
                $timeout(function () {
                    uploadNewMusic();
                });
            }
        }
        
        function editMusic(music) {
            appModals.editMusicModal(music, $scope.antiForgeryToken, $scope.tags, $scope.search);
        }

        function uploadNewMusic() {
            $scope.openEditMusicPanel({});
        }

        function musicEditInit() {
            searchService.getTags()
                .then(getTagsSuccess, getTagsFailed);
        }
        function getTagsSuccess(evt) {
            $scope.tags = evt.data
                .map(function (tg) {
                    tg.hirarychyStr = tg.TagHirarchi.reverse().join(' ← ');
                    return tg;
                });
            $scope.tags.forEach(function (tag) {
                tag.TagList = [];
                tagDictionary[tag.ID] = tag;
            });
            $scope.tags.forEach(function (tag) {
                if (tag.ParentTagID) {
                    var parent = tagDictionary[tag.ParentTagID];
                    parent.TagList.push(tag);
                    tag.ParentTag = parent;
                }
            });
            $scope.tags = $scope.tags.filter(function (tag) {
                return (tag.Level > 1 || !tag.TagList.length) && tag.IsPublicTag
            });
            
            $scope.tagsLoaded();
        }

        function getTagsFailed(evt) {
            appModals.openAlertModal('פסקול - היצירות שלי','אירעה שגיאה בהבאת התגיות');
        }

        function search() {
            var data = { sortByDateDesc: $scope.sort.sortType === '1', page: $scope.pager.page };
            musicService.mySongs(data)
                .then(searchSuccess, searchFailed);
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
        function musicResultsInit() {
            $scope.sort = { sortType: '1' };
            $scope.results = [];
            $scope.pager = {page : 1};
            $scope.TotalResults = 0;
            $scope.TotalPages = 0;
            $scope.sortOptions = [{ text: 'מחדש לישן', value: '1' },
                                { text: 'מישן לחדש', value: '2' }];
            $scope.searchLoad = false;
            $scope.isArtistSongsModel = true;
            $scope.hideSycnVideoBtn = true;
        }
    }
})();