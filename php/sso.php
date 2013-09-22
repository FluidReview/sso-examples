<?php
	$url = "https://[YOUR SUBDOMAIN].fluidreview.com";
	$signin_url = $url . '/api/signin/';
	$auth_url = $url . '/api/authenticate/';

	$email = 'you@example.com';
	$password = 'password';

	$req = curl_init();

	curl_setopt($req, CURLOPT_URL, $auth_url);
	curl_setopt($req, CURLOPT_RETURNTRANSFER, true);
	curl_setopt($req, CURLOPT_POSTFIELDS, 'email=' . urlencode($email) . '&password=' . urlencode($password));

	$result = curl_exec($req);

	$data = json_decode($result, true);

	$token = $data['token'];

	$req = curl_init();

	curl_setopt($req, CURLOPT_URL, $signin_url);
	curl_setopt($req, CURLOPT_RETURNTRANSFER, true);
	curl_setopt($req, CURLOPT_POSTFIELDS,
		'email=' . urlencode('applicant@example.com') .
		'&group=7' .
		'&first_name=John' .
		'&last_name=Doe' .
		'&token=' . $token 
	);

	$result = curl_exec($req);

	$data = json_decode($result, true);

	print $data['return_path']

?>