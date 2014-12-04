module.exports = {
  specs: ['app/js/**/spec/**/*.js'],
  templates: ['app/js/**/templates/**/*.html'],
  scripts: ['app/js/**/module.js', 'app/js/**/*.js', '!app/js/**/spec/**/*.js'],
  sass: ['app/scss/**/*.scss', 'app/js/**/css/**/*.scss']
};
