/// <reference path="../angular.min.js" />
var app = angular.module('MyApp', ['ngRoute', 'ui.bootstrap', 'ang-drag-drop']);

app.config(function ($routeProvider) {
    $routeProvider

        // by default, redirect to home page
        .when('/', {
            redirectTo: '/Home',
            data: {
                privateData: true
            }
        })

        // route for the home page
        .when('/Home', {
            templateUrl: 'Views/Home.html',
            data: {
                privateData: true
            }
        })
        .when('ForgotPasswordModal', {
            templateUrl: 'Views/ForgotPasswordModal.html',
            controller: 'ResetPasswordController',
            data: {
                privateData: true
            }
        })
        // route for the courses page
        .when('/Courses', {
            templateUrl: 'Views/Courses.html',
            controller: 'CourseController',
            data: {
                privateData: false
            }
        })
        // route for the user courses page
        .when('/UserCourses', {
            templateUrl: 'Views/UserCourses.html',
            controller: 'UserCoursesController',
            data: {
                privateData: false
            }
        })
        // route for the create courses page
        .when('/CreateCourse', {
            templateUrl: 'Views/CreateCourse.html',
            controller: 'CreateCourseController',
            data: {
                privateData: false
            }
        })
        // route for the edit courses page
        .when('/EditCourse/:courseId', {
            templateUrl: 'Views/EditCourse.html',
            controller: 'EditCourseController',
            data: {
                privateData: false
            }
        })
        //route for my page
        .when('/MyPage', {
            templateUrl: 'Views/MyPage.html',
            controller: 'MyPageController',
            data: {
                privateData: false
            }
        })
        //route for email sent page
        .when('/EmailSentPage', {
            templateUrl: 'Views/EmailSentPage.html',
            data: {
                privateData: true
            }
        })
        //
        .when('/EmailConfirmed', {
            templateUrl: 'Views/EmailConfirmed.html',
            controller: 'EmailConfirmedController',
            data: {
                privateData: true
            }
        })
        //route for tutor page
        .when('/Tutor/:wordSuiteId', {
            templateUrl: 'Views/Tutor.html',
            controller: 'TutorController',
            data: {
                privateData: false
            }
        })
        //route for wordsuite words page
        .when('/WordSuiteWords/:wordSuiteId', {
            templateUrl: 'Views/WordSuiteWords.html',
            controller: 'WordSuiteWordsController',
            data: {
                privateData: false
            }
        })
        // route for the Create WordSuite page
        .when('/CreateWordSuite', {
            templateUrl: 'Views/CreateWordSuite.html',
            controller: 'CreateWordSuiteController',
            data: {
                privateData: false
            }
        })
        // route for the Edit WordSuite page
        .when('/EditWordSuite', {
            templateUrl: 'Views/EditWordSuite.html',
            controller: 'EditWordSuiteController',
            data: {
                privateData: false
            }
        })
        // route for configuring group
        .when('/AddGroup', {
            templateUrl: 'Views/AddGroup.html',
            controller: 'AddGroupController',
            data: {
                privateData: false
            }
        })

        // route for showing words
        .when('/Words', {
            templateUrl: 'Views/Words.html',
            controller: 'WordsController',
            data: {
                privateData: false
            }
        })

        // route for adding words
        .when('/AddWord', {
            templateUrl: 'Views/AddWord.html',
            controller: 'WordsController',
            data: {
                privateData: false
            }
        })

        // route for the languages page
        .when('/Languages', {
            templateUrl: 'Views/Languages.html',
            controller: 'LanguageController',
            data: {
                privateData: false
            }
        })

        // route for the quiz page
        .when('/Quiz/:wordSuiteId', {
            templateUrl: 'Views/Quiz.html',
            controller: 'QuizController',
            data: {
                privateData: false
            }
        })
        // route for the groups page 
        .when('/Groups', {
            templateUrl: 'Views/Groups.html',
            controller: 'GroupController',
            data: {
                privateData: false
            }
        })
        // route for the wordsuites page 
        .when('/WordSuites', {
            templateUrl: 'Views/WordSuites.html',
            controller: 'WordSuitesController',
            data: {
                privateData: false
            }
            //route for the import wordtranslations page
        }).when('/ImportWordTranslations', {
            templateUrl: 'Views/ImportWordTranslations.html',
            controller: 'ImportWordTranslationsController',
            data: {
                privateData: false
            }
        })
        //route for creating wordtranslations
        .when('/CreateWordTranslation', {
            templateUrl: 'Views/CreateWordTranslation.html',
            controller: 'CreateWordTranslationController'
        })
        // route for configuring group page
        .when('/Groups/:groupId', {
            templateUrl: 'Views/GroupDetails.html',
            controller: 'GroupDetailsController',
            data: {
                privateData: false
            }
        })
        // route for progress chart
        .when('/Groups/:groupId/:userId/chart', {
            templateUrl: 'Views/StudentProgressChart.html',
            controller: 'StudentProgressChartController',
            data: {
                privateData: false
            }
        })
        // route for progress of student
        .when('/Groups/:groupId/:userId', {
            templateUrl: 'Views/StudentProgress.html',
            controller: 'StudentProgressController',
            data: {
                privateData: false
            }
        })
        // route for editing wordsuite page
        .when('/EditWordSuite/:wordSuiteId', {
            templateUrl: 'Views/EditWordSuite.html',
            controller: 'EditWordSuiteController',
            data: {
                privateData: false
            }
        })
        .when('/EditUserProfile', {
            templateUrl: 'Views/EditUserProfile.html',
            controller: 'EditUserProfileController',
            data: {
                privateData: false
            }
        })
        //route for course detail
        .when('/CourseDetail/:Id', {
            templateUrl: 'Views/CourseDetail.html',
            controller: 'CourseDetailController',
            data: {
                privateData: false
            }
        })
        //route for list of users
        .when('/Users', {
            templateUrl: 'Views/Users.html',
            controller: 'UsersConfiguringController',
            data: {
                privateData: false
            }
        })
        //route for Global Dictionary
        .when('/GlobalDictionary', {
            templateUrl: 'Views/GlobalDictionary.html',
            controller: 'GlobalDictionaryController',
            data: {
                privateData: false
            }
        })
        .when('RegisterModal', {
            templateUrl: 'Views/RegisterModal.html',
            controller: 'RegisterController',
            data: {
                privateData: true
            }
        })
        .when('/ChangePassword', {
            templateUrl: 'Views/ChangePassword.html',
            controller: 'ChangePasswordController',
            data: {
                privateData: true
            }
        })
        //route for Global Dictionary
        .when('/GlobalDictionary', {
            templateUrl: 'Views/GlobalDictionary.html',
            controller: 'GlobalDictionaryController',
            data: {
                privateData: false
            }
        })
        .when('LoginModal', {
            templateUrl: 'Views/LoginModal.html',
            controller: 'LoginController',
            data: {
                privateData: true
            }
        })
        .when('UnassignedModal', {
            templateUrl: 'Views/UnassignedModal.html',
            controller: 'UnassignedModalController',
            data: {
                privateData: false
            }
        })
        .when('/PasswordChanged', {
            templateUrl: 'Views/PasswordChanged.html',
            data: {
                privateData: true
            }
        })
        .when('/UnassignedModal',{
            templateUrl: 'Views/UnassignedModal.html',
            data: {
                privateData: false
            }
        })
        .when('/TeacherManager', {
            templateUrl: 'Views/TeacherManager.html',
            controller: 'TeacherManagerController',
            data: {
                privateData: false
            }
    });
});

app.run(function ($rootScope, $modal, UserService, ConstService) {
    $rootScope.$on("$routeChangeSuccess", function (event, next) {
        if (!UserService.getUserData()) {
            $rootScope.isPrivate = next.data.privateData;
            if (!$rootScope.isPrivate) {
                location.replace('Index#/Home');
                $modal.open({
                    templateUrl: '../Views/UnauthorizeModal.html',
                    controller: 'UnauthorizeModalController',
                    size: ConstService.small
                });
                $rootScope.isPrivate = true;
            } else {
                $rootScope.isPrivate = true;
            };
        };
    });
});