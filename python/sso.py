import json

from urllib import urlencode
from urllib2 import urlopen

fluidreview_url = "http://[YOUR SUBDOMAIN].fluidreview.com" # Your FluidReview URL
signin_path = '/api/signin/'
auth_path = '/api/authenticate/'

signin_url = '%s%s' % (fluidreview_url, signin_path)
auth_url = '%s%s' % (fluidreview_url, auth_path)

admin_email = 'you@example.com' # Your site's owner's email address
admin_password = 'password' # Your site's owner's password

'''
Authenticate with the API. This will return a rolling token
that should be included with all subsequent requests.

API authentication details can be found at:
http://test01.fluidreview.com/api/docs/#authentication
'''

credentials = urlencode({
	'email': admin_email,
	'password': admin_password,
})

'''
Send our credentials via HTTP POST, this will authenticate with the FluidReview API.
'''

response = urlopen(auth_url, credentials).read()

token = json.loads(response)['token']

'''
The following values are required by the `sign in` API method.
The most important field being `email`, which will govern what
user this is.
'''

data = urlencode({
	'email': 'applicant@example.com', # Email address of applicant
	'group': '7', # ID of applicant group to assign this applicant to
	'first_name': 'John', # First name of applicant
	'last_name': 'Doe', # Last name of applicant
	'token': token, # API token,
	'identifier': 'student_id',
	'student_id': '2435454654',
})

'''
Send the information about our user to the FluidReview API's sign-in method.
If a user with the given email exists, a one-time-login URL will be returned.

If a user account with the given email does not exist, a new account will be created.

Both scenarios will return the same response.
'''

response = urlopen(signin_url, data).read()

return_path = json.loads(response)['return_path']

'''
The client should now be redirected to `return_path`, via an HTTP redirect or other means.

`return_path` is a use-once URL that will automatically sign the client in to FluidReview.
'''

print return_path

