(function () {
    'use strict';

    angular
        .module('passkolApp')
        .controller('searchByTagController', searchByTagController);

    searchByTagController.$inject = ['$scope', 'searchService', 'musicService', 'appModals', 'appConfig', '$timeout', 'logService'];

    function searchByTagController($scope, searchService, musicService, appModals, appConfig, $timeout, logService) {
        // Data
        $scope.tags = [];
        $scope.tagDictionary = {};
        var searchResults = [];
        $scope.tagsFilter = { Level: 1 };
        $scope.searchText = '';
        $scope.selectedTag;
        $scope.selectedTags = [];
        $scope.selectableTags = [];
        $scope.selectedIds = [];
        $scope.selectedTagsIdsStr = '';
        var timeoutPromise;

        // Music Results Data
        function musicResultInit() {
            $scope.results = [];
            $scope.pager = { page : 1};
            $scope.TotalResults = 0;
            $scope.TotalPages = 0;
            $scope.sortOptions = [{ text: 'רלוונטיות', value: '1' },
                                { text: 'אורך (מהקצר לארוך)', value: '2' },
                                { text: 'אורך (מהארוך לקצר)', value: '3' }];
            $scope.sort = { sortType: '1' };
            $scope.searchLoad = false;
        }

        // Initilaize
        musicResultInit();
        load();

        // Methods
        $scope.tagClick = tagClick;
        $scope.reloadSearch = reloadSearch;
        $scope.search = startSearch;

        // Methods implement    
        function tagClick(tag) {
            $scope.selctedSecondTag = {};
            if (tag) {
                if (tag.TagList.length) {
                    $scope.selectedTag = tag;
                    $scope.tagsFilter = { ParentTagID: tag.ID };
                } else {
                    tag.isSelected = !$scope.selectedTags.some(function (tg) { return tg.ID === tag.ID });
                    $scope.tagSelectCheked(tag);
                }
            } else {
                $scope.selectedTag = tag;
                $scope.tagsFilter = { Level: 1 };
            }
        }

        function handleLocation(path, searchParams) {
            for (var param in searchParams) {
                switch (param.toLowerCase()) {
                    case ('id'):
                        $scope.musicId = searchParams[param];
                        searchMusic($scope.musicId);
                        break;
                    case ('strtagids'):
                        $scope.selectedIds = searchParams[param]
                            .split(';')
                            // Remove empties
                            .filter(function (id) { return id });
                        $scope.selectedTagsIdsStr = $scope.selectedIds.join(';');
                        break;
                    case ('searchtext'):
                        $scope.searchText = searchParams[param];
                        break;
                }
            }

            $scope.layout = path.startsWith('/search')
                ? 'searchMode' : 'indexMode';
            $scope.indexMode = $scope.layout === 'indexMode';
            $scope.searchMode = $scope.layout === 'searchMode';
        }

        function load() {
            getLocationData();

            searchService.getTags().then(function success(evt) {
                $scope.tags = evt.data.map(function (tg) {
                    tg.hirarychyStr = tg.TagHirarchi.reverse().join(' ← ');
                    return tg;
                });

                buildTagHirarchy();
                getSelectedTagsFromIds();
                if ($scope.searchText || $scope.selectedTagsIdsStr) {
                    startSearch();
                } else {
                    $scope.searchLoad = true;
                }
            }, function failed() {
                appModals.alertModal('פסקול', 'אירעה שגיאה!');
            });
        }

        function getLocationData() {
            var loactionParams = location.search.substr(1).split('&');
            var serchObj = {};
            loactionParams.forEach(function (search) {
                var keyValue = search.split('=');
                serchObj[keyValue[0]] = decodeURI(keyValue[1]);
            });

            handleLocation(location.pathname.toLowerCase(), serchObj);
        }

        function buildTagHirarchy() {
            $scope.tags.forEach(function (tag) {
                tag.TagList = [];
                $scope.tagDictionary[tag.ID] = tag;
            });
            $scope.tags.forEach(function (tag) {
                if (tag.ParentTagID) {
                    var parent = $scope.tagDictionary[tag.ParentTagID];
                    parent.TagList.push(tag);
                    tag.ParentTag = parent;
                }
            });
        }

        function getSelectedTagsFromIds() {
            $scope.selectedTags = $scope.selectedIds.map(function (id) {
                var tg = $scope.tagDictionary[id];
                if (tg) {
                    tg.isSelected = true;
                }

                return tg;
            }).filter(function (tg) {
                return tg;
            });
            $scope.selectableTags = $scope.tags.filter(function (tag) {
                return !tag.TagList.length || $scope.selectedTags.find(function (tg) {
                    return tag.ID == tg.ID;
                });
            });
            $scope.selectableTagsChange($scope.selectableTags);
            $scope.selectedTagsChange($scope.selectedTags);
        }

        function searchMusic(id) {
            musicService.musicById(id).then(function success(evt) {
                evt.data.isNew = evt.data.Tags && !!evt.data.Tags.find(function (tg) {
                    return tg.Level === 1 && tg.Name === 'חדש במערכת';
                });
                $scope.results = [evt.data];
                $scope.TotalResults = 1;
                $scope.TotalPages = 1;
                $scope.pages = new Array($scope.TotalPages);

                for (var i = 0; i < $scope.pages.length; i++) {
                    $scope.pages[i] = i;
                }
            }, function failed(evt) {
                if (evt.status !== 404) {
                    appModals.alertModal('פסקול', 'אירעה שגיאה בחיפוש');
                }
            });
        }

        function startSearch() {
            if (timeoutPromise) {
                $timeout.cancel(timeoutPromise);
            }
            if (!$scope.musicId) {
                $scope.searchLoad = false;

                searchService
                .sendSearch($scope.searchText, $scope.selectedTagsIdsStr, $scope.pager.page, $scope.sort.sortType)
                .then(searchSuccess, searchFailed);
            }
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

            timeoutPromise = $timeout(logSearch, 30 * 1000, true, $scope.searchText, $scope.selectedIds, $scope.TotalResults);
            $scope.searchLoad = true;
        }
        
        function logSearch(searchText, selectedIds, TotalResults) {
            if (appConfig.userConfig.isAuthenticated &&
                $scope.sort.sortType == '1') {
                if (selectedIds && selectedIds.length || searchText) {
                    var searchData = {
                        searchString: searchText,
                        tags: selectedIds,
                        resultCount: TotalResults
                    };
                }
                
                logService.logSearch(searchData);
            }
        }

        function searchFailed(res) {
            $scope.searchLoad = true;
            appModals.alertModal('פסקול', 'אירעה שגיאה בחיפוש');
        }

        function reloadSearch() {
            if ($scope.layout === 'indexMode') {
                $scope.layout = 'searchMode';
                $scope.indexMode = $scope.layout === 'indexMode';
                $scope.searchMode = $scope.layout === 'searchMode';
            }
            $scope.pager.page = 1;
            $scope.sort.sortType = 1;
            delete $scope.musicId;
            if ($scope.searchText || $scope.selectedTagsIdsStr) {
                //location = '/Search?searchText=' + $scope.searchText +
                //    '&strTagIds=' + $scope.selectedTagsIdsStr;
                startSearch();
            } else {
                $scope.results = [];
            }
        }
    }
})();
