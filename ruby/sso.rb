require 'uri'
require 'json'
require 'net/http'

fluidreview_url = 'http://[YOUR SUBDOMAIN].fluidreview.com' # Your FluidReview URL
signin_path = '/api/signin/'
auth_path = '/api/authenticate/'

signin_url = "#{fluidreview_url}#{signin_path}"
auth_url = "#{fluidreview_url}#{auth_path}"

admin_email = 'you@example.com' # Your site's owner's email address
admin_password = 'password' # Your site's owner's password

uri = URI(auth_url)
res = Net::HTTP.post_form(uri,
	'email' => admin_email,
	'password' => admin_password
)

token = JSON.parse(res.body)['token']

uri = URI(signin_url)
res = Net::HTTP.post_form(uri,
	'email' => 'applicant@example.com', # Email address of applicant
	'group' => '7', # ID of applicant group to assign this applicant to
	'first_name' => 'John', # First name of applicant
	'last_name' => 'Doe', # Last name of applicant
	'token' => token, # API token,
	'identifier' => 'student_id',
	'student_id' => '2435454654'
)

puts JSON.parse(res.body)['return_path']
