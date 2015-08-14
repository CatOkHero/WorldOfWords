app.service('WordSuiteService', function ($http, $q, HttpRequest) {
    this.createWordSuite = function (wordSuite) {
        var deferred = $q.defer();
        HttpRequest.post("api/wordsuite/CreateWordSuite", wordSuite, deferred);
        return deferred.promise;
    }

    this.editWordSuite = function (wordSuite) {
        var deferred = $q.defer();
        HttpRequest.post("api/wordsuite/EditWordSuite", wordSuite, deferred);
        return deferred.promise;
    }

    this.getWordSuitesByOwnerID = function (id) {
        var deferred = $q.defer();
        HttpRequest.get("api/wordsuite/GetTeacherWordSuites?id=" + id, deferred);
        return deferred.promise;
    }

    this.getWordSuitesByLanguageID = function (languageId) {
        var deferred = $q.defer();
        HttpRequest.get("api/wordsuite/GetWordSuitesByLanguageId?languageId=" + languageId, deferred);
        return deferred.promise;
    }

    this.getWordSuiteByID = function (id) {
        var deferred = $q.defer();
        HttpRequest.get("api/wordsuite/GetWordSuiteByID?id=" + id, deferred);
        return deferred.promise;
    }

    this.getWordsFromWordSuite = function (id) {
        var deferred = $q.defer();
        HttpRequest.get("api/TrainingWordSuite/AllWords?id=" + id, deferred);
        return deferred.promise;
    }

    this.getPrintVersion = function (id) {
        var deferred = $q.defer();
        HttpRequest.get('api/wordsuite/' + id + '/pdf', deferred);
        return deferred.promise;
    }

    this.addWordProgresses = function (wordProgresses) {
        var deferred = $q.defer();
        HttpRequest.post("api/wordsuite/AddWordProgresses", wordProgresses, deferred);
        return deferred.promise;
    }

    this.removeWordProgress = function (wordProgress) {
        var deferred = $q.defer();
        HttpRequest.post("api/wordsuite/RemoveWordProgresses", wordProgress, deferred);
        return deferred.promise;
    }

    this.deleteWordSuite = function (id) {
        var deferred = $q.defer();
        HttpRequest.delete("api/WordSuite?id=" + id, deferred);
        return deferred.promise;
    }
})