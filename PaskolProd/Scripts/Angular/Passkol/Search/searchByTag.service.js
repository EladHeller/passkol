(function () {
    angular.module("passkolApp").factory("searchService", SearchMusicService);
    SearchMusicService.$inject = ['$http'];
    function SearchMusicService($http) {
        var service = {
            getTags: getTags,
            sendSearch: sendSearch
        };

        return service;

        function sendSearch(text, tagsStr, page, sortType) {
            return $http.post('Search/Search?searchText=' + text + '&strTagIds=' + tagsStr + '&page=' + page + '&sortType=' + sortType);
        }

        function getTags() {
            return $http.post('/search/tags');
        }
    }
})();