var http = require('http');
var request = require('request');
var querystring = require('querystring');

var url = "https://[YOUR SUBDOMAIN].fluidreview.com";
var signinPath = url + '/api/signin/';
var authPath = url + '/api/authenticate/';

var email = 'you@example.com';
var password = 'password;';

var auth = function() {
	request.post(authPath, {form: {
			'email': email,
			'password': password
		}}, function (error, response, body) {
			if (!error && response.statusCode == 200) {
				signIn(JSON.parse(body).token);
			}
		}
	);
};

var signIn = function(token) {
	console.log(token);
	request.post(signinPath, {form: {
			email: 'applicant@example.com',
			group: '7',
			first_name: 'John',
			last_name: 'Doe',
			token: token,
			identifier: 'student_id',
			student_id: '2435454654'

		}}, function (error, response, body) {
			if (!error && response.statusCode == 200) {
				console.log(JSON.parse(body).return_path);
			}
		}
	);
};

auth();
