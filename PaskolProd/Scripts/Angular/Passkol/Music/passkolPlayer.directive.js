(function() {
    'use strict';

    angular
        .module('passkolApp')
        .directive('passkolPlayer', passkolPlayerDirective);

    passkolPlayerDirective.$inject = ['logService','appConfig','$window','$timeout'];
    
    function passkolPlayerDirective(logService, appConfig, $window, $timeout) {
        var directive = {
            link: link,
            restrict: 'E',
            templateUrl: 'passkolPlayer.template'
        };
        return directive;

        function link(scope, element, attrs) {
            // Events
            scope.$watch(getCurrPlayedMusicId, onPlayedMusicChanged);

            // Methods
            scope.setPlayedMusic = setPlayedMusic;
            scope.closePlayedMusic = closePlayedMusic;
            scope.nextMusic = nextMusic;
            scope.previousMusic = previousMusic;
            scope.showVideoBtnClicked = showVideoBtnClicked;

            // Methods implementation
            function showVideoBtnClicked() {
                scope.showVideoSync = !scope.showVideoSync;
                if (scope.showVideoSync) {
                    $timeout(function () {
                        $window.scrollTo(0, 10000);
                    });
                }
            }

            function setPlayedMusic(music) {
                scope.showAudioPlayer = true;
                scope.musicPlayed = music;
                scope.playClicked();
            }

            function closePlayedMusic() {
                scope.showAudioPlayer = false;
                if (!scope.audioPlayer.paused) {
                    scope.playClicked();
                }
            }

             function nextMusic() {
                var musicIndex = scope.results.indexOf(scope.musicPlayed);

                if (scope.results.length > (musicIndex + 1)) {
                    scope.musicPlayed = scope.results[musicIndex + 1];
                }
            }

            function previousMusic() {
                var musicIndex = scope.results.indexOf(scope.musicPlayed);
                
                if (musicIndex > 0) {
                    scope.musicPlayed = scope.results[musicIndex - 1];
                }
            }

            function getCurrPlayedMusicId() {
                return scope.musicPlayed && scope.musicPlayed.ID;
            }

            function onPlayedMusicChanged(newValue, oldValue) {
                if (newValue && newValue !== oldValue) {
                    var sources = [];
                    if (scope.musicPlayed.MP3FileID) {
                        var file = scope.musicPlayed.MP3File;
                        sources.push(location.origin + '/File/GetRange?FileId=' + file.FileId + '&UserId=' + scope.musicPlayed.ArtistID + '&FileType=Music&version=' + file.version + '&suffix=' + file.suffix);
                    }
                    if (scope.musicPlayed.WAVFileID) {
                        var file = scope.musicPlayed.WAVFile;
                        sources.push(location.origin + '/File/GetRange?FileId=' + file.FileId + '&UserId=' + scope.musicPlayed.ArtistID + '&FileType=Music&version=' + file.version + '&suffix=' + file.suffix);
                    }
                    if (sources.length) {
                        logShowMusic();
                        scope.changeAudioSource(sources);
                    } else {
                        scope.audioError = true;
                    }
                }
            }

            function logShowMusic() {
                var logData = {
                    EntityId: scope.musicPlayed.ID,
                    EntityType: 'Music',
                    ActionType: 'Listen'
                };
                logService.logAction(logData);
            }
        }
    }
})();