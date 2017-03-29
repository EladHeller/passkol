(function() {
    'use strict';

    angular
        .module('passkolApp')
        .directive('editMusic', editMusicDirective);

    editMusicDirective.$inject = ['appModals', 'musicService'];
    
    function editMusicDirective(appModals, musicService) {
        var directive = {
            link: link,
            restrict: 'E',
            templateUrl: 'editMusic.template'
        };
        return directive;

        function link(scope, element, attrs) {
            // Data
            var autoCompleteTagsElement = element.find('#autoCompleteTags');
            scope.showValidationErrors = false;
            scope.showEditMusicPanel = false;
            scope.editedMusic = {};
            scope.href = scope.href || {};
            scope.href.path = 'MySongs';
            scope.copyrightsTypeOptions = [
                { id: 1, name: 'כותב' },
                { id: 2, name: 'מלחין' },
                { id: 3, name: 'מעבד' },
                { id: 0, name: 'מו\"ל' }
            ];
            var select2TagsWidget;

            // Methods
            scope.updateSumCopyrights = updateSumCopyrights;
            scope.addCopyright = addCopyright;
            scope.clearCopyrights = clearCopyrights;
            scope.removeCopyright = removeCopyright;
            scope.openEditMusicPanel = openEditMusicPanel;
            scope.save = save;
            scope.tagsLoaded = tagsLoaded;

            // Init
            init();

            // Methods implement
            function init() {
                if (scope.tags) {
                    tagsLoaded();
                }
                if (scope.music) {
                    openEditMusicPanel(scope.music);
                }
            }

            function beforeUnloadAlert(e) {
                var alertMessage = 'המוזיקה שהעלית בתהליך העלאה.\n אם תצא המוזיקה לא תישמר,';
                (e || window.event).returnValue = alertMessage;     //Gecko + IE
                return alertMessage;                                //Webkit, Safari, Chrome etc.
            }

            function save(saveAndClose) {
                var validRes = validateForm();
                if (!validRes.isValid) {
                    appModals.alertModal('ניהול יצירות', validRes.message);
                } else {
                    scope.uploadMusicLoading = true;
                    window.addEventListener("beforeunload", beforeUnloadAlert);
                    scope.editedMusic.Copyrights.forEach(function (copyright) {
                        copyright.Type = copyright.selectedType.id;
                    });
                    return musicService.uploadMusic(scope.editedMusic, scope.WAVFile, scope.MP3File, select2TagsWidget.val(), scope.antiForgeryToken)
                        .then(function success(evt) {
                            window.removeEventListener("beforeunload", beforeUnloadAlert)
                            scope.uploadMusicLoading = false;
                            if (evt.data.Success) {
                                scope.search();
                                scope.showEditMusicPanel = !saveAndClose;
                                resetForm();
                            } else {
                                appModals.alertModal('ניהול יצירות',evt.data.message);
                            }
                        }, function failed(evt) {
                            window.removeEventListener("beforeunload", beforeUnloadAlert)
                            scope.uploadMusicLoading = false;
                            appModals.alertModal('ניהול יצירות', 'אירעה שגיאה!');
                        });
                }

                return validRes.isValid;
            }
            function resetForm() {
                scope.editedMusic = {};
                scope.editedMusic.Copyrights = [];
                scope.showValidationErrors = false;
                scope.resetWAVFile();
                scope.resetMP3File();
                select2TagsWidget.val([]);
            }
            function validateForm() {
                var res = {
                    isValid: false,
                    message:''
                };
                scope.showValidationErrors = true;
                updateSumCopyrights();
                if (!scope.editMusicFrm.$valid) {
                    res.message = 'לא כל שדות החובה מולאו!';
                } else if (!(scope.MP3File || scope.editedMusic.MP3FileID)) {
                    res.message = 'יש להעלות קובץ MP3!';
                } else if (!(scope.WAVFile || scope.editedMusic.WAVFileID)) {
                    res.message = 'יש להעלות קובץ WAV!';
                } else if (scope.editedMusic.sumPercents !== 100) {
                    res.message = 'סך זכויות היוצרים צריכות להיות 100%!';
                } else if (scope.editedMusic.Minutes === 0 && scope.editedMusic.Seconds === 0) {
                    res.message = 'יש להזין אורך יצירה!';
                } else {
                    res.isValid = true;
                }

                return res;
            }

            function tagsLoaded() {
                var convertedData = scope.tags.map(function (tag) {
                    return { id: tag.ID, text: tag.hirarychyStr };
                });
                select2TagsWidget = autoCompleteTagsElement.select2({
                    data: convertedData,
                    dir: 'rtl',
                    placeholder: "חפש תגיות"
                });
            }
            function addCopyright() {
                var newCopyright = {
                    MusicID: scope.editedMusic.ID,
                    Type: 0,
                    Percents: 0,
                    CopyrightAuthorName: '',
                    selectedType: scope.copyrightsTypeOptions[0]
                };
                scope.editedMusic.Copyrights.push(newCopyright);
                updateSumCopyrights();
            }
            function clearCopyrights() {
                scope.editedMusic.Copyrights = [];
                updateSumCopyrights();
            }
            function updateSumCopyrights() {
                var sumPercents = 0;
                scope.editedMusic.Copyrights.forEach(function (copyright) {
                    sumPercents += copyright.Percents;
                });
                scope.editedMusic.sumPercents = sumPercents;
            }
            function removeCopyright(copyright) {
                var indexToRemove = scope.editedMusic.Copyrights.indexOf(copyright);
                scope.editedMusic.Copyrights.splice(indexToRemove, 1);
                updateSumCopyrights();
            }
            
            function openEditMusicPanel(music) {
                scope.showEditMusicPanel = true;
                scope.editedMusic = music;
                if (scope.editedMusic) {
                    scope.editedMusic.sumPercents = 0;
                    if (!scope.editedMusic.Copyrights) {
                        scope.editedMusic.Copyrights = [];
                    } else {
                        scope.editedMusic.Copyrights = scope.editedMusic.Copyrights
                            .map(function (copyright) {
                                scope.editedMusic.sumPercents += copyright.Percents;
                                copyright.selectedType = scope.copyrightsTypeOptions.find(function (option) {
                                    return option.id === copyright.Type;
                                });
                                return copyright;
                            });
                    } 
                    if (scope.editedMusic.Tags && scope.editedMusic.Tags.length) {
                        var tagIds = scope.editedMusic.Tags.map(function (tag) {
                            return tag.ID;
                        });

                        select2TagsWidget.val(tagIds).trigger('change');
                    } else {
                        select2TagsWidget.val([]).trigger('change');
                    }
                }
            }
        }
    }
})();