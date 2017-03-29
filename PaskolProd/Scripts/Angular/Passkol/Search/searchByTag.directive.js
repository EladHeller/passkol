(function () {
    'use strict';

    angular
        .module('passkolApp')
        .directive('searchByTag', searchByTagDirective);

    searchByTagDirective.$inject = ['$document'];

    function searchByTagDirective($document) {
        var select2;
        var scope = {

        };
        var directive = {
            link: link,
            restrict: 'E',
            templateUrl: 'searchByTag.template'
        };
        return directive;

        function link(scope, element, attrs) {
            // Data
            var searchBtn = element.find('.megasearchbtn');

            // init
            select2 = element.find('#autoCompleteTag').select2({
                dir: 'rtl'
            });
            var searchContainerOffset = element.find('.select2-selection--multiple').offset().left;
            var searchInputOffset = element.find('.select2-search__field').offset().left;
            
            // events
            element.find('#autoCompleteTag').on('select2:select', selectedTagChange)
            element.find('#autoCompleteTag').on('select2:unselect', selectedTagChange)
            element.find('#autoCompleteTag').on('select2:open', selectWindowOpen)

            // methods
            scope.selectableTagsChange = selectableTagsChange;
            scope.selectedTagsChange = selectedTagsChange;
            scope.tagSelectCheked = tagSelectCheked;

            // methods implement
            function selectWindowOpen() {
                searchInputOffset = element.find('.select2-search__field').offset().left;
                searchContainerOffset = element.find('.select2-selection--multiple').offset().left;
                var innerInputOffset = searchInputOffset - searchContainerOffset - 240 + element.find('.select2-search__field').width();
                var select2Dropdown = $('.select2-dropdown');
                select2Dropdown.css('margin-left', innerInputOffset + 'px');
            }
            function selectedTagChange(evt) {

                scope.tagDictionary[evt.params.data.id].isSelected = evt.params.data.selected;
                scope.selectedIds = select2.val() || [];
                scope.selectedTagsIdsStr = scope.selectedIds ? scope.selectedIds.join(';') : '';
                scope.reloadSearch();
                scope.$apply();
            }

            function selectableTagsChange(data) {
                var convertedData = data.map(function (tag) {
                    return { id: tag.ID, text: tag.hirarychyStr };
                });
                select2 = select2.select2({
                    data: convertedData,
                    dir: 'rtl'
                });
            }

            function tagSelectCheked(tag) {
                if (tag.isSelected) {
                    scope.selectedTags.push(tag);
                    scope.selectedIds.push(tag.ID);
                    if (!scope.selectableTags.some(function (x) { return x.ID === tag.ID })) {
                        scope.selectableTags.push(tag);
                        selectableTagsChange(scope.selectableTags);
                    }
                    select2.val(scope.selectedIds).trigger('change');
                } else {
                    scope.selectedTags.splice(scope.selectedTags.indexOf(tag), 1);
                    scope.selectedIds.splice(scope.selectedIds.indexOf(tag.ID), 1);
                    select2.val(scope.selectedIds).trigger('change');
                    if (tag.TagList.length) {
                        scope.selectableTags.splice(scope.selectableTags.indexOf(tag), 1);
                        selectableTagsChange(scope.selectableTags);
                    }
                }

                scope.selectedTagsIdsStr = scope.selectedIds.join(';');

                scope.reloadSearch();
            }
            function selectedTagsChange(data) {
                select2.val(data.map(function (tag) {
                    return tag.ID;
                })).trigger('change');
            }
        }
    }
})();