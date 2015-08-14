app.controller('IndexController',
    function ($http,
        $scope,
        $window,
        $modal,
        IndexService,
        UserService,
        EditUserProfileSevice,
        ConstService) {
        var userName = EditUserProfileSevice.getUserName()
            .then(function (result) {
                userName = result;
                $scope.userName = userName;
            });
        var userInfo = UserService.getUserData();
        $scope.showIcon = true;
        $scope.showWordSuitesIcon = true;
        $scope.sideBarIsMinimized = false;
        $scope.isLoggedIn = function () {
            return !!userInfo;
        };
        $scope.getName = userName;
        $scope.isStudent = userInfo && userInfo.roles.indexOf(ConstService.StudentRole) >= 0;
        $scope.isTeacher = userInfo && userInfo.roles.indexOf(ConstService.TeacherRole) >= 0;
        $scope.isAdmin = userInfo && userInfo.roles.indexOf(ConstService.AdminRole) >= 0;
        $scope.onLogoClick = function () {
            if (userInfo) {
                if (userInfo.roles.indexOf(ConstService.StudentRole) >= 0) {
                    location.replace('/Index#/Courses/');
                } else {
                    if (userInfo.roles.indexOf(ConstService.TeacherRole) >= 0) {
                        location.replace('Index#/TeacherManager');
                    } else {
                        if (userInfo.roles.indexOf(ConstService.AdminRole) >= 0)
                            location.replace('Index#/Users');
                    };
                };
            };
        };
        if (userInfo) {
            if (userInfo.roles.indexOf(ConstService.TeacherRole) >= 0) {
                $scope.showWordSuitesIcon = false;
            };
            $scope.userIconURL = userInfo.id + '.png';
        };
        $scope.logOut = function () {
            UserService.setUserData(null);
            $http.defaults.headers.common.Authorization = null;
            userInfo = UserService.getUserData();
            location.replace('/Index#/');
        };
        $scope.openLoginModal = function () {
            var modalInstance = $modal.open({
                templateUrl: '../Views/LoginModal.html',
                controller: 'LoginController',
                size: ConstService.small
            });
            modalInstance.result.then();
        };
        $scope.openRegisterModal = function () {
            var modalInstance = $modal.open({
                templateUrl: '../Views/RegisterModal.html',
                controller: 'RegisterController',
                size: ConstService.small
            });
            modalInstance.result.then();
        };
        $scope.toggleSidebar = function () {
            $scope.sideBarIsMinimized = !$scope.sideBarIsMinimized;
            $scope.showIcon = !$scope.showIcon;
        };
        $scope.getBodySidebarClass = function () {
            return !$scope.isLoggedIn() ? ConstService.sidebarClosed
                : $scope.sideBarIsMinimized ? ConstService.sidebarMinimized : null;
        };
        $scope.showCoursePage = function () {
            if (ConstService.coursePath === document.URL) {
                $scope.showCourseList = false;
            } else {
                location.replace('Index#/Courses');
                $scope.showCourseList = true;
            };
        };
        $scope.showTeacherManager = function () {
            if (ConstService.teacherPath === document.URL) {
                $scope.showManagerList = false;
            } else {
                location.replace('Index#/TeacherManager');
                $scope.showManagerList = true;
            };
        };
        $scope.showManagersList = function () {
            $scope.showManagerList = false;
        };
        $scope.showCoursesList = function () {
            $scope.showCourseList = false;
        };
    });