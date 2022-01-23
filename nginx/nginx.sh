sudo cp nginx/localhost.crt /etc/ssl/certs/
sudo cp nginx/localhost.key /etc/ssl/private/
# haslo do ssl_password_file
sudo cp nginx/global.pass /etc/keys/
sudo cp nginx/default /etc/nginx/sites-enabled/
# start nginx
sudo apt-get install nginx
sudo service nginx reload