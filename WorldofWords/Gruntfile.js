module.exports = function (grunt) {
    //grunt.loadNpmTasks('grunt-karma');

    grunt.initConfig({
        pkg: grunt.file.readJSON("package.json"),
 
        uglify: {
            "Minification": {
                files: {
                    "dist/build.min.js": [
                        "Scripts/app/Controllers/*.js",
                        "Scripts/app/Services/*.js",
                        "Scripts/app/Filters/*.js",
                        "Scripts/app/Directives/*.js"]
                }
            }
        },

        less: {
            styles: {
                files: [
                    {
                        "Content/styles.css": "Content/Syte.less"
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
