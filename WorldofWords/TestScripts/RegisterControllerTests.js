describe('RegisterController', function () {
    beforeEach(module('MyApp'));

    var $controller;
    var registerServiceMock = {}
    var $scope;

    beforeEach(module(function ($provide) {
        // Replace a real service with a mock
        $provide.value('RegisterService', registerServiceMock);
    }));

    beforeEach(inject(function (_$controller_, $q, $rootScope) {
        // The injector unwraps the underscores (_) from around the parameter names when matching
        $controller = _$controller_;
        // Create a new child scope for each test
        $scope = $rootScope.$new();
        registerServiceMock.registerUser = function (userInfo) {
            var deferred = $q.defer();
            if (userInfo.email === 'fail@fail.com') {
                deferred.resolve(-1);
            }
            else {
                deferred.resolve(1);
            }
            return deferred.promise;
        };
    }));

    describe('$scope.loginButtonClick', function () {
        it('sets errorMessage to "Enter name" if nameValue is empty', function () {
            $scope.nameValue = '';
            $scope.passwordValue = 'pass';
            $scope.repeatPasswordValue = 'pass';
            $scope.emailValue = 'example@example.com';
            var controller = $controller('RegisterController', { $scope: $scope });

            $scope.loginButtonClick();

            expect($scope.errorMessage).toEqual('Enter name');
        });
        it('sets errorMessage to "Passwords are different" if passwords mismatch', function () {
            $scope.nameValue = 'name';
            $scope.passwordValue = 'pass';
            $scope.repeatPasswordValue = 'ssap';
            $scope.emailValue = 'example@example.com';
            var controller = $controller('RegisterController', { $scope: $scope });

            $scope.loginButtonClick();

            expect($scope.errorMessage).toEqual('Passwords are different');
        });
        it('sets errorMessage to "User with specified e-mail already exists" if passwords mismatch', function () {
            $scope.nameValue = 'name';
            $scope.passwordValue = 'pass';
            $scope.repeatPasswordValue = 'pass';
            $scope.emailValue = 'fail@fail.com';
            var controller = $controller('RegisterController', { $scope: $scope });

            $scope.loginButtonClick();
            $scope.$root.$digest();

            expect($scope.errorMessage).toEqual('User with specified e-mail already exists');
        });
        it('Changes the location to MyPage if registration is successful', function () {
            $scope.nameValue = 'name';
            $scope.passwordValue = 'pass';
            $scope.repeatPasswordValue = 'pass';
            $scope.emailValue = 'success@success.com';
            var controller = $controller('RegisterController', { $scope: $scope });
            spyOn(window.location, 'replace');

            $scope.loginButtonClick();
            $scope.$root.$digest();

            expect(window.location.replace).toHaveBeenCalledWith("/Index#/MyPage");
        });
    });
});