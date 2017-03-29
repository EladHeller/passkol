(function() {
    'use strict';

    angular
        .module('passkolApp')
        .directive('faq', faq);

    faq.$inject = ['$location','$timeout'];
    
    function faq($location, $timeout) {
        var directive = {
            link: link,
            restrict: 'E'
        };
        return directive;

        function link(scope, element, attrs) {
            var search = $location.search();
            if (search.tab) {
                scope.tab = search.tab;
                var path = $location.path();
                var question = element.find('#' + path.substr(1));

                var answer = element.find('#' + path.substr(1) + ' + .collapseme');
                answer.toggle();
                $timeout(function () {
                    window.scrollTo(0, question.position().top);
                });
            }
            
        }
    }

})();