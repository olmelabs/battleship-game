if (process.env.NODE_ENV === 'production'){
  module.exports = require('./api.config.prod');
} else {
  module.exports = require('../api/api.config.dev');
}
