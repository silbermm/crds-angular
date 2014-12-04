angular.module('crdsProfile').factory('Profile', [
    '$http', '$q', function ($http, $q) {
        return {
            get: function () {
                var deferred = $q.defer();
                $http.get('/api/profile/5')
                    .success(deferred.resolve)
                    .error(deferred.reject);
                return deferred.promise;
            }
        }
    }
]);