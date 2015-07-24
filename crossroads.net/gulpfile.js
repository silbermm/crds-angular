var gulp = require("gulp");
var watch = require("gulp-watch");
var gutil = require("gulp-util");
var webpack = require("webpack");
var gulpWebpack = require("gulp-webpack");
var WebpackDevServer = require("webpack-dev-server");
var webpackConfig = require("./webpack.config.js");
var webpackCoreConfig = require("./webpack.core.config.js");
var webpackDependenciesConfig = require("./webpack.dependencies.config.js");
var svgSprite = require("gulp-svg-sprite");
var replace = require("gulp-replace");
var rename = require("gulp-rename");

var browserSyncCompiles = 0;
var browserSync = require('browser-sync').create();

var webPackConfigs = [Object.create(webpackDependenciesConfig), Object.create(webpackCoreConfig), Object.create(webpackConfig)];

// Start the development server
gulp.task("default", ["webpack-dev-server"]);

// Build and watch cycle (another option for development)
// Advantage: No server required, can run app from filesystem
// Disadvantage: Requests are not blocked until bundle is available,
//               can serve an old app on refresh
gulp.task("build-dev", ["webpack:build-dev"], function() {

	var watchPatterns = [];
	webPackConfigs.forEach(function(element) {
		watchPatterns.push(element.watchPattern);
		gutil.log("Adding watch", element.watchPattern);
	});

	gulp.watch(watchPatterns, ["webpack:build-dev"]);
});

gulp.task('build-browser-sync', function () {
	webPackConfigs.forEach(function(element) {

		element.devtool = "eval";
		element.debug = true;
		element.output.path = "/";

		// force gulpWebpack to watch for file changes
		element.watch = true;

		// Build app to assets - watch for changes
		gulp.src(element.watchPattern)
			.pipe(gulpWebpack(element))
			.pipe(gulp.dest("./assets"));
	});
});

// Browser-Sync build
// May be useful for live injection of SCSS / CSS changes for UI/UX
// Also should reload pages when JS / HTML are regenerated
gulp.task("browser-sync-dev", ["build-browser-sync"], function() {

	// Watch for final assets to build
	gulp.watch("./assets/*.js", function() {
		gutil.log("JS files in assets folder modified", "Count = " + browserSyncCompiles);

		if (browserSyncCompiles >= webPackConfigs.length) {
			gutil.log("Forcing BrowserSync reload");
			browserSync.reload();
		}

		browserSyncCompiles += 1;
	});

	browserSync.init({
		server: {
			baseDir: "./"
		}
	});
});

// Production build
gulp.task("build", ["webpack:build"]);

// For convenience, an "alias" to webpack-dev-server
gulp.task("start", ["webpack-dev-server"]);


// Run the development server
gulp.task("webpack-dev-server", ["icons-watch"], function(callback) {
	webPackConfigs.forEach(function(element, index) {

		// Modify some webpack config options
		element.devtool = "eval";
		element.debug = true;
		element.output.path = "/";
		// Build app to assets - watch for changes
		gulp.src("app/**/**")
			.pipe(watch(element.watchPattern))
			.pipe(gulpWebpack(element))
			.pipe(gulp.dest("./assets"));
	});

	new WebpackDevServer(webpack(webPackConfigs), {
			historyApiFallback: {
			  index: 'index.html',
			  rewrites: [
				// TODO: see if there is a way to dry this up so we don't need to specify every folder/filename
				{ from: /\/corkboard\/assets\/main.js/, to: '/corkboard/assets/main.js'},
				{ from: /\/corkboard/, to: '/corkboard/index.html'}
			  ]
			},
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

gulp.task("webpack:build", ["icons"], function(callback) {
	webPackConfigs.forEach(function(element) {
		// modify some webpack config options
		element.plugins = element.plugins.concat(
			new webpack.DefinePlugin({
				"process.env": {
					// This has effect on the react lib size
					"NODE_ENV": JSON.stringify("production")
				}
			}),
			new webpack.optimize.DedupePlugin()
			// Can't currently use this with our angular code
			// This is probably due to not fully following $inject or inline annotation, or using a plugin like ngMinPlugin
			//new webpack.optimize.UglifyJsPlugin()
		);

		// TODO: Remove once we fully support Uglification for all JS files
		// This caused an issue with the #/give/ page showing an A with an accent character between GIVE and $100 in the button. Commenting out again.
		//if (element.entry.dependencies) {
		//	gutil.log("[start]", "adding additional plugins for " +  JSON.stringify(element.entry.dependencies));
        //
		//	element.plugins = element.plugins.concat(
		//		new webpack.optimize.UglifyJsPlugin()
		//	);
		//}
	});

	// run webpack
	webpack(webPackConfigs, function(err, stats) {
		if(err) throw new gutil.PluginError("webpack:build", err);
		gutil.log("[webpack:build]", stats.toString({
			colors: true
		}));
		callback();
	});
});

gulp.task("webpack:build-dev", ["icons"], function(callback) {
	webPackConfigs.forEach(function(element) {
		// modify some webpack config options
		element.devtool = "sourcemap";
		element.debug = true;
	});

	// run webpack
	webpack(webPackConfigs).run(function(err, stats) {
		if(err) throw new gutil.PluginError("webpack:build-dev", err);
		gutil.log("[webpack:build-dev]", stats.toString({
			colors: true
		}));
		callback();
	});
});

// Watches for svg icon changes - run "icons" once, then watch
gulp.task("icons-watch", ["icons"], function() {
	gulp.watch("app/icons/*.svg", ["icons"]);
});

// Builds sprites and previews for svg icons
gulp.task("icons", ["svg-sprite"], function() {
    gulp.src('build/icons/generated/defs/sprite.defs.html')
	  .pipe(rename("preview-svg.html"))
      .pipe(gulp.dest('./assets'));

    gulp.src('build/icons/generated/defs/svg/sprite.defs.svg').pipe(rename("cr.svg")).pipe(gulp.dest('./assets'));
});


gulp.task("svg-sprite", function() {
	var config = {
		log: "info",
		mode: {
			defs: {
				prefix: ".icon-%s",
				example: {
					template: __dirname + "/config/sprite.template.html",
				},
				inline: true,
				bust: false
			}
		}
	};

	return gulp.src("./app/icons/*.svg")
		.pipe(svgSprite(config))
		.pipe(gulp.dest("./build/icons/generated"));
});
