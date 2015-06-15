var webpack = require("webpack");
var ExtractTextPlugin = require("extract-text-webpack-plugin");
var path = require("path");

var endpoint = {
    'url': 'http://localhost:49380'
};

var definePlugin = new webpack.DefinePlugin({
    __API_ENDPOINT__: JSON.stringify(process.env.CRDS_API_ENDPOINT || "http://gatewayint.crossroads.net/gateway/"),
    __CMS_ENDPOINT__: JSON.stringify(process.env.CRDS_CMS_ENDPOINT || "http://contentint.crossroads.net/"),
    __STRIPE_PUBKEY__ : JSON.stringify(process.env.CRDS_STRIPE_PUBKEY || "pk_test_TR1GulD113hGh2RgoLhFqO0M")
});

module.exports = {
    entry: {
        main: './app/app.js'
    },
    watch: 'app/**/**',
    externals: {
      stripe: "Stripe",
      angular: "angular",
      moment: "moment"

    },
    context: __dirname,
    output: {
        path: './assets',
        publicPath: '/assets/',
        filename: '[name].js'
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
                  path.resolve(__dirname, "app"),
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

      //{ test: /\.html$/,loader: "ngtemplate?module=myTemplates&relativeTo=^" + (path.resolve(__dirname, 'app/')) + "!html"}
    ]
    },
    plugins: [
        new ExtractTextPlugin("[name].css"),
        definePlugin
    ]
};
