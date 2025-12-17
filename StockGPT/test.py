import requests

url = ('https://newsapi.org/v2/everything?q=AAPL&from=2024-10-19&sortBy=popularity&apiKey=3b3859b565604d55919a20c5421747a2')

response = requests.get(url)

print(response.json())