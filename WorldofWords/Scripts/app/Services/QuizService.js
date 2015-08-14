app.service('QuizService', function ($http, $q, HttpRequest) {
    this.getWords = function (id) {
        var deferred = $q.defer();
        HttpRequest.get("api/TrainingWordSuite/Task?id=" + id, deferred);
        return deferred.promise;
    }

    this.sendResult = function (data) {
        var deferred = $q.defer();
        HttpRequest.post('/api/TrainingWordSuite', data, deferred);
        return deferred.promise;
    }
})