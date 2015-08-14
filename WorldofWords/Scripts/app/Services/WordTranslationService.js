app.service('WordTranslationService', function ($http, $q, HttpRequest) {
    this.searchForWordTranslations = function (searchWord, languageId) {
        var deferred = $q.defer();
        HttpRequest.get('api/WordTranslation?searchWord=' + searchWord + '&languageId=' + languageId, deferred);
        return deferred.promise;
    }

    this.getWordTranslationsByWordSuiteID = function (ID) {
        var deferred = $q.defer();
        HttpRequest.get('api/WordTranslation?ID=' + ID, deferred);
        return deferred.promise;
    }

    this.importWordTranslations = function (wordTranslations) {
        var deferred = $q.defer();
        HttpRequest.post('api/WordTranslation/ImportWordTranslations', wordTranslations, deferred);
        return deferred.promise;
    }

    this.addWordTranslation = function (wordtranslation) {
        var deferred = $q.defer();
        HttpRequest.post('/api/WordTranslation/CreateWordTranslation', wordtranslation, deferred);
        return deferred.promise;
    }

    this.getWordTranslationsFromInterval = function (start, end, lang) {
        var deferred = $q.defer();
        HttpRequest.get('/api/GlobalDictionary?start=' + start+'&end='+ end + '&language=' + lang, deferred);
        return deferred.promise;
    }

    this.getAmountOfWordTranslationsBySpecificLanguage = function (lang) {
        var deferred = $q.defer();
        HttpRequest.get('/api/GlobalDictionary?languageID=' + lang, deferred);
        return deferred.promise;
    }

    this.getAmountOfWordTranslationBySearchValue = function (searchValue, lang) {
        var deferred = $q.defer();
        HttpRequest.get('/api/GlobalDictionary?searchValue=' + searchValue + '&languageId=' + lang, deferred);
        return deferred.promise;
    }

    this.getWordTranslationBySearchValueFromInterval = function(start, end, lang, searchValue)
    {
        var deferred = $q.defer();
        HttpRequest.get('/api/GlobalDictionary?searchValue='+searchValue + '&startOfInterval=' + start + '&endOfInterval=' + end + '&languageId=' + lang, deferred);
        return deferred.promise;
    }
})