var gulp = require("gulp");
var watch = require("gulp-watch");
var gutil = require("gulp-util");
var webpack = require("webpack");
var gulpWebpack = require("gulp-webpack");
var WebpackDevServer = require("webpack-dev-server");
var webpackConfig = require("./webpack.config.js");

// Start the development server
gulp.task("default", ["webpack-dev-server"]);

// Build and watch cycle (another option for development)
// Advantage: No server required, can run app from filesystem
// Disadvantage: Requests are not blocked until bundle is available,
//               can serve an old app on refresh
gulp.task("build-dev", ["webpack:build-dev"], function() {
	gulp.watch(["app/**/*"], ["webpack:build-dev"]);
});

// Production build
gulp.task("build", ["webpack:build"]);

// For convenience, an "alias" to webpack-dev-server
gulp.task("start", ["webpack-dev-server"]);

// Run the development server
gulp.task("webpack-dev-server", function(callback) {
	// modify some webpack config options
	var myConfig = Object.create(webpackConfig);
	myConfig.devtool = "eval";
	myConfig.debug = true;
	myConfig.output.path = "/";

	// Build app to assets - watch for changes
	gulp.src("app/**/**")
		.pipe(watch("app/**/**"))
		.pipe(gulpWebpack(myConfig))
		.pipe(gulp.dest("./assets"));

	new WebpackDevServer(webpack(myConfig), {
			publicPath: "/assets/",
			quiet: false,
			watchDelay: 300,
			stats: {
				colors: true
			}
			}).listen(8080, "localhost", function(err) {
				if(err) throw new gutil.PluginError("webpack-dev-server", err);
				gutil.log("[start]", "http://localhost:8080/webpack-dev-server/index.html");
			});
			
	gutil.log("[start]", "Access crossroads.net at http://localhost:8080/#");
	gutil.log("[start]", "Access crossroads.net Live Reload at http://localhost:8080/webpack-dev-server/#");
});

gulp.task("webpack:build", function(callback) {
	// modify some webpack config options
	var myConfig = Object.create(webpackConfig);
	myConfig.plugins = myConfig.plugins.concat(
		new webpack.DefinePlugin({
			"process.env": {
				// This has effect on the react lib size
				"NODE_ENV": JSON.stringify("production")
			}
		}),
		new webpack.optimize.DedupePlugin()
		//new webpack.optimize.UglifyJsPlugin()
	);

	// run webpack
	webpack(myConfig, function(err, stats) {
		if(err) throw new gutil.PluginError("webpack:build", err);
		gutil.log("[webpack:build]", stats.toString({
			colors: true
		}));
		callback();
	});
});

gulp.task("webpack:build-dev", function(callback) {
	// modify some webpack config options
	var myDevConfig = Object.create(webpackConfig);
	myDevConfig.devtool = "sourcemap";
	myDevConfig.debug = true;

	// run webpack
	webpack(myDevConfig).run(function(err, stats) {
		if(err) throw new gutil.PluginError("webpack:build-dev", err);
		gutil.log("[webpack:build-dev]", stats.toString({
			colors: true
		}));
		callback();
	});
});
