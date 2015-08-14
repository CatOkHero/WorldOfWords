module.exports = function (grunt) {
    //grunt.loadNpmTasks('grunt-karma');

    grunt.initConfig({
        pkg: grunt.file.readJSON("package.json"),

        uglify: {
            "WebApp27": {
                files: {
                    "dist/Controllers.min.js": [
                        "WorldofWords/Scripts/app/Controllers/CourseController.js",
                        "WorldofWords/Scripts/app/Controllers/HomeController.js",
                        "WorldofWords/Scripts/app/Controllers/LoginController.js",
                        "WorldofWords/Scripts/app/Controllers/MyPageController.js",
                        "WorldofWords/Scripts/app/Controllers/RegisterController.js"]
                }
            }
        },

        less: {
            styles: {
                files: [
                    {
                        "WorldofWords/Content/styles.css": "WorldofWords/Content/Syte.less"
                    }
                ]
            }
        }

        //karma: {
        //    unit: {
        //        configFile: 'WorldofWords/karma.conf.js'
        //    }
        //}

    });

    grunt.loadNpmTasks("grunt-contrib-uglify");
    grunt.loadNpmTasks("grunt-contrib-less");

    grunt.registerTask("default", ["uglify", "less"]);
    //grunt.registerTask('unittest', ['karma:unit', 'karma:unit:run']);

};