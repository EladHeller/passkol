(function () {
    angular.module("passkolApp").factory("musicService", musicService);
    musicService.$inject = ['$http'];
    function musicService($http) {
        var service = {
            addToFavouriteMusic: addToFavouriteMusic,
            shareInMail: shareInMail,
            musicById: musicById,
            musicsByArtistId: musicsByArtistId,
            getTransactUrl: getTransactUrl,
            musicsByFavouriteMusic: musicsByFavouriteMusic,
            mySongs: mySongs,
            uploadMusic:uploadMusic
        };

        return service;

        function uploadMusic(editedMusic, WAVFile, MP3File, tagIds, antiForgeryToken) {
            var fd = new FormData();
            if (!tagIds) {
                tagIds = '';
            }
            fd.append('editedMusic', JSON.stringify(editedMusic));
            fd.append('WAVFile', WAVFile);
            fd.append('MP3File', MP3File);
            fd.append('tagIdsStr', tagIds);
            return $http({
                method: 'POST',
                url: '/music/SaveMusic',
                data: fd,
                transformRequest: angular.identity,
                headers: {
                    'RequestVerificationToken': antiForgeryToken,
                    'X-Requested-With': 'XMLHttpRequest',
                    'Content-Type': undefined
                }
            });
        }

        function addToFavouriteMusic(music, antiForgoten) {
            return $http({
                method: 'POST',
                url: '/music/MusicFavourite',
                data: { id: music.ID },
                headers: {
                    'RequestVerificationToken': antiForgoten,
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
        }

        function shareInMail(data, antiForgoten) {
            return $http({
                method: 'POST',
                url: '/music/ShareInMail',
                data: data,
                headers: {
                    'RequestVerificationToken': antiForgoten,
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
        }

        function musicById(id) {
            return $http({
                method: 'POST',
                url: '/music/GetByID?id=' + id ,
            });
        }

        function musicsByArtistId(data) {
            return $http.post('/music/MusicByArtistId',data)
        }

        function getTransactUrl(antiForgoten) {
            return $http({
                method: 'POST',
                url: '/Transact/GetUrl',
                headers: {
                    'RequestVerificationToken': antiForgoten,
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
        }
        function musicsByFavouriteMusic(data) {
            return $http.post('/Music/FavouriteMusics', data);
        }

        function mySongs(data) {
            return $http.post('/Music/MySongs', data);
        }
    }
})();