app.service('MyPageService', function ($http, $q, HttpRequest) {
    this.getUserInfo = function (userId) {
        var deferred = $q.defer();
        HttpRequest.get("/api/MyPage/" + userId, deferred);
        return deferred.promise;
    }
})