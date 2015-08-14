app.service('LanguageService', function ($http, $q, HttpRequest) {
    this.getAllLanguages = function () {
        var deferred = $q.defer();
        HttpRequest.get('/api/language', deferred);
        return deferred.promise;
    }
    this.addLanguage = function (language) {
        var deferred = $q.defer();
        HttpRequest.post('/api/language', language, deferred);
        return deferred.promise;
    }
    this.removeLanguage = function (id) {
        var deferred = $q.defer();
        HttpRequest.delete('/api/language?id=' + id, deferred);
        return deferred.promise;
    }
})