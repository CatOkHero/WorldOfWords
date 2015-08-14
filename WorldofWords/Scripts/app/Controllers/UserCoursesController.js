app.controller('UserCoursesController', function ($modal, $scope, $window, CourseService, UserService) {

    $scope.topCount = 3;

    $scope.confirmRemoving = function (course) {
        $scope.courseId = course.Id;
        $scope.open('confirmModal', $scope.removeCourse);
    }
    $scope.removeCourse = function () {
        CourseService.removeCourse($scope.courseId)
                .then(function (response) {
                    $scope.init();
                },
            function (error) {
                $scope.open('errorModal', $scope.goToCourses, (error.ExceptionMessage || error.Message || "Unknown error"));
            });
    };

    $scope.open = function (url, successCallback, data, errorCallback) {
        var modalInstance = $modal.open({
            animation: true,
            templateUrl: url,
            controller: 'ModalController',
            size: 'sm',
            resolve: {
                data: function () {
                    return data;
                }
            }
                });
        modalInstance.result.then(successCallback, errorCallback);
    };

    $scope.init = function () {
        CourseService.getUserCourses()
        .then(function (response) {
            $scope.courses = response;
        });
    };

    $scope.init();



});

