/// <reference path="../node_modules/karma-jasmine/lib/jasmine.js" />
describe('LoginController', function () {
    beforeEach(module('MyApp'));

    var $controller;
    var loginServiceMock = {}
    var $scope;

    beforeEach(module(function ($provide) {
        // Replace a real service with a mock
        $provide.value('LoginService', loginServiceMock);
    }));

    beforeEach(inject(function (_$controller_, $q, $rootScope) {
        // The injector unwraps the underscores (_) from around the parameter names when matching
        $controller = _$controller_;
        // Create a new child scope for each test
        $scope = $rootScope.$new();
        loginServiceMock.login = function (userInfo) {
            var deferred = $q.defer();
            if (userInfo.email === 'fail@fail.com') {
                deferred.resolve('');
            }
            else {
                deferred.resolve('Great success!');
            }
            return deferred.promise;
        };
    }));

    describe('$scope.loginButtonClick', function () {
        
        it('sets errorMessage to "User with specified e-mail already exists" if passwords mismatch', function () {
            $scope.email = 'fail@fail.com';
            $scope.password = 'pass';
            var controller = $controller('LoginController', { $scope: $scope });

            $scope.loginButtonClick();
            $scope.$root.$digest();

            expect($scope.errorMessage).toEqual('');
        });
        it('Changes the location to MyPage if login is successful', function () {
            $scope.email = 'success@success.com';
            $scope.password = 'pass';
            var controller = $controller('LoginController', { $scope: $scope });
            spyOn(window.location, 'replace');

            $scope.loginButtonClick();
            $scope.$root.$digest();

            expect(window.location.replace).toHaveBeenCalledWith("/Index#/MyPage");
        });
        it('sets errorMessage to "Wrong user e-mail or password" if passwords mismatch', function () {
            $scope.email = '';
            $scope.password = '';
            var controller = $controller('LoginController', { $scope: $scope });

            $scope.loginButtonClick();
            $scope.$root.$digest();

            expect($scope.errorMessage).toEqual('Wrong e-mail or password');
        });
    });
});