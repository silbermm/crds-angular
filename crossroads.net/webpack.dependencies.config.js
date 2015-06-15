var webpack = require("webpack");
var ExtractTextPlugin = require("extract-text-webpack-plugin");
var path = require("path");

var endpoint = {
    'url': 'http://localhost:49380'
};

var definePlugin = new webpack.DefinePlugin({
    __API_ENDPOINT__: JSON.stringify(process.env.CRDS_API_ENDPOINT || "http://mp-int-web.cloudapp.net/gateway/"),
    __CMS_ENDPOINT__: JSON.stringify(process.env.CRDS_CMS_ENDPOINT || "http://content.crossroads.net/"),
    __STRIPE_PUBKEY__ : JSON.stringify(process.env.CRDS_STRIPE_PUBKEY || "pk_test_TR1GulD113hGh2RgoLhFqO0M")
});

module.exports = {
    entry: {
        dependencies: "./dependencies/dependencies.js"
    },
    watch: 'dependencies/**/**',
    externals: {
      stripe: "Stripe"
    },
    context: __dirname,
    output: {
        path: './assets',
        publicPath: '/assets/',
        filename: '[name].js',
        sourceMapFilename: '[name].js.map'
    },
    module: {
        loaders: [
            {
                test: /\.css$/,
                loader: "style-loader!css-loader"
            },
            {
                test: /\.js$/,
                include: [
                  path.resolve(__dirname, "dependencies"),
                  path.resolve(__dirname, "node_modules/angular-stripe")
                ],
                loader: 'babel-loader'
            },
            {
                test: /\.scss$/,
                loader: ExtractTextPlugin.extract('style-loader', 'css-loader!autoprefixer-loader!sass-loader')
            },
            {
                test: /\.(jpe?g|png|gif|svg)$/i,
                loaders: ['image?bypassOnDebug&optimizationLevel=7&interlaced=false']
            },
            {
                test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                loader: "url-loader?limit=10000&minetype=application/font-woff"
            },
            {
                test: /\.(ttf|eot|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                loader: "file-loader"
            },
            {
                test: /\.html$/,
                loader: "ng-cache?prefix=[dir]"
            }
    ]
    },
    plugins: [
        new ExtractTextPlugin("[name].css"),
        definePlugin
    ]
};
