(function() {
    'use strict';

    angular
        .module('passkolApp')
        .directive('musicResults', musicResultsDirective);

    musicResultsDirective.$inject = ['appConfig','appModals','musicService', '$timeout','logService'];
    
    function musicResultsDirective(appConfig, appModals, musicService, $timeout, logService) {
        var directive = {
            link: link,
            restrict: 'E',
            templateUrl: 'musicResults.template'
        };
        return directive;

        function link(scope, element, attrs) {
            // Data
            scope.selectedOption = scope.selectedOption || scope.sortOptions && scope.sortOptions[0];
            scope.showOptions = false;
            
            var resultsTimeoutPermission;

            // Methods
            scope.purchaseMusic = purchaseMusic;
            scope.favouriteMusic = favouriteMusic;
            scope.openResult = openResult;
            scope.openSharedModal = openSharedModal;
            scope.pageSearch = pageSearch;
            scope.optionSelect = optionSelect;

            // Events
            scope.$watch(function () {
                return scope.results;
            }, onResultChanged);

            // Methods implementation
            function onResultChanged(newValue, oldValue) {
                if (newValue !== oldValue) {
                    if (resultsTimeoutPermission) {
                        $timeout.cancel(resultsTimeoutPermission);
                    }
                    if (newValue && newValue.length) {
                        resultsTimeoutPermission = $timeout(logResults, 30 * 1000);
                    }
                }
            }

            function pageSearch(page) {
                if (scope.pager.page !== page) {
                    scope.pager.page = page;
                    scope.search();
                }
            }

            function optionSelect(option) {
                scope.selectedOption = option;
                scope.sort.sortType = option.value;
                scope.search();
            }

            function openSharedModal(music) {
                appModals.shareMusicModal(music, scope.antiForgeryToken);
            }

            function purchaseMusic(music) {
                if (!appConfig.userConfig.isAuthenticated || appConfig.userConfig.userType !== 'Customer') {
                    appModals.alertModal('פסקול', 'התחבר למערכת פסקול כרוכש תוכן על מנת שתוכל לרכוש יצירות!');
                } else {
                    appModals.selectPermissionModal(music);
                }
            }

            function favouriteMusic(music) {
                if (!appConfig.userConfig.isAuthenticated) {
                    appModals.alertModal('פסקול', 'התחבר למערכת פסקול על מנת שתוכל להוסיף יצירות למועדפים!');
                } else {
                    musicService.addToFavouriteMusic(music, scope.antiForgeryToken)
                        .then(function (evt) { addFavouriteMusicSuccess(evt,music) }, addFavouriteMusicFailed);
                }
            }

            function addFavouriteMusicFailed(evt) {
                appModals.alertModal('פסקול', 'אירעה שגיאה בהוספת היצירה למועדפים.');
            }

            function addFavouriteMusicSuccess(evt,music) {
                if (evt.data.Success) {
                    music.IsFavourite = !music.IsFavourite;
                } else {
                    var statusResponse = JSON.parse(evt.headers('X-Responded-JSON'));
                    if (statusResponse) {
                        if (statusResponse.status === 401) {
                            appModals.alertModal('פסקול', 'התחבר למערכת פסקול על מנת שתוכל להוסיף יצירות למועדפים!');
                        }
                    } else {
                        appModals.alertModal('פסקול', 'אירעה שגיאה בהוספת היצירה למועדפים.');
                    }
                }
            }

            function openResult(result, $event) {
                if (!result.isOpen && !$event.target.classList.contains("play-music-btn") && 
                    $event.target.nodeName.toUpperCase() !== 'IMG') {
                    result.isOpen = true;
                } else if ($event.target.classList.contains("closetr")) {
                    result.isOpen = false;
                }
            }

            function logResults() {
                var logsData = scope.results.map(function (result) {
                    return {
                        EntityId: result.ID,
                        EntityType: 'Music',
                        ActionType: 'Watch'
                    };
                });

                logService.logActions(logsData);
            }
        }
    }
})();