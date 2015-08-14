/// <reference path="WordsService.js" />
app.service('WordsService', function ($http, $q, HttpRequest) {
    this.getWordsByLanguageId = function (id) {
        var deferred = $q.defer();
        HttpRequest.get('/api/words?languageId=' + id, deferred);
        return deferred.promise;
    }
    this.addWord = function (word) {
        var deferred = $q.defer();
        HttpRequest.post('/api/Words', word, deferred);
        return deferred.promise;
    }

    this.getWordById = function (id) {
        var deferred = $q.defer();
        HttpRequest.get('/api/Words?wordId=' + id, deferred);
        return deferred.promise;
    }

    this.searchForWords = function (searchWord, languageId, searchResultsCount) {
        var deferred = $q.defer();
        HttpRequest.get('api/Words?searchWord='+searchWord+'&languageId='+languageId+'&searchResultsCount='+searchResultsCount, deferred);
        return deferred.promise;
    }

    this.searchForTranslations = function (searchWord, count) {
        var deferred = $q.defer();
        HttpRequest.get('api/Words?searchWord=' + searchWord + '&languageId=4&searchResultsCount='+count, deferred);
        return deferred.promise;
    }
})