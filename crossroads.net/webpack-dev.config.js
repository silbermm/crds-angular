var webpack = require('webpack');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var path = require('path');

var endpoint = {
  url: 'http://localhost:49380'
};

var definePlugin = new webpack.DefinePlugin({
  __API_ENDPOINT__: JSON.stringify(process.env.CRDS_API_ENDPOINT || 'https://gatewayint.crossroads.net/gateway/'),
  __CMS_ENDPOINT__: JSON.stringify(process.env.CRDS_CMS_ENDPOINT || 'https://contentint.crossroads.net/'),
  __STRIPE_PUBKEY__: JSON.stringify(process.env.CRDS_STRIPE_PUBKEY || 'pk_test_TR1GulD113hGh2RgoLhFqO0M'),
  __SOUNDCLOUD_API_KEY__: JSON.stringify(process.env.CRDS_SOUNDCLOUD_KEY || '67723f3ff9ea6bda29331ac06ce2960c'),
  __AWS_SEARCH_ENDPOINT__ :
    JSON.stringify(process.env.CRDS_AWS_SEARCH_ENDPOINT ||
       'https://vs9gac5tz7.execute-api.us-east-1.amazonaws.com/prod/')
});

module.exports = {
  entry: {
    trips: './app/trips/trips.module.js',
    search: './app/search/search.module.js',
    media: './app/media/media.module.js',
    give: './app/give/give.module.js',
    profile: './app/profile/profile.module.js',
    main: './app/app.js',
    ang: './app/ang.js',
    core: ['./node_modules/crds-core'],
    common: ['./app/common/common.module.js'],
  },
  watchPattern: 'app/**/**',
  externals: {
    stripe: 'Stripe',
    moment: 'moment'
  },
  context: __dirname,
  output: {
    path: './assets',
    publicPath: '/assets/',
    filename: '[name].js',
  },
    devtool: 'sourcemap',
    debug: true,
    module: {
        loaders: [
            {
                test: /\.css$/,
                loader: 'style-loader!css-loader'
            },
            {
                test: /\.js$/,
                include: [
                  path.resolve(__dirname, 'app'),
                  path.resolve(__dirname, 'node_modules/angular-stripe')
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
                loader: 'url-loader?limit=10000&minetype=application/font-woff'
            },
            {
                test: /\.(ttf|eot|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                loader: 'file-loader'
            },
            {
                test: /\.html$/,
                loader: 'ng-cache?prefix=[dir]'
            }
    ]
    },
    plugins: [
        new ExtractTextPlugin('[name].css'),
        definePlugin
    ]
  };
