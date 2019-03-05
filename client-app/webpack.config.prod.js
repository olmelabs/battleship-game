import webpack from "webpack";
import path from "path";
import MiniCssExtractPlugin from "mini-css-extract-plugin";
import UglifyJsPlugin from "uglifyjs-webpack-plugin";
import CopyWebpackPlugin from "copy-webpack-plugin";

const GLOBALS = {
  "process.env.NODE_ENV": JSON.stringify("production")
};

export default {
  // devtool: "source-map",
  mode: "production",
  entry: path.resolve(__dirname, "src/index"),
  target: "web",
  output: {
    path: __dirname + "/dist", // Note: Physical files are only output by the production build task `npm run build`.
    publicPath: "/",
    filename: "bundle.js"
  },
  devServer: {
    contentBase: "./dist"
  },
  plugins: [
    new webpack.optimize.OccurrenceOrderPlugin(),
    new webpack.DefinePlugin(GLOBALS),
    new MiniCssExtractPlugin({ filename: "styles.css" }),
    new CopyWebpackPlugin([
      {
        from: path.join(__dirname, "src/assets/images/favicon.ico"),
        to: "favicon.ico"
      }
    ])
  ],
  module: {
    rules: [
      {
        test: /\.js$/,
        include: path.join(__dirname, "src"),
        loaders: ["babel-loader"]
      },
      { test: /(\.css)$/, use: [MiniCssExtractPlugin.loader, "css-loader"] },
      { test: /\.eot(\?v=\d+\.\d+\.\d+)?$/, loader: "file-loader" },
      { test: /\.(woff|woff2)$/, loader: "url?prefix=font/&limit=5000" },
      {
        test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/,
        loader: "url?limit=10000&mimetype=application/octet-stream"
      },
      {
        test: /\.svg(\?v=\d+\.\d+\.\d+)?$/,
        loader: "url?limit=10000&mimetype=image/svg+xml"
      }
    ]
  },
  optimization: {
    minimizer: [
      new UglifyJsPlugin({
        cache: true,
        parallel: true,
        sourceMap: true // set to true if you want JS source maps
      })
      //,new OptimizeCSSAssetsPlugin({})
    ]
  }
};
