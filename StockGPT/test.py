import requests

url = ('https://newsapi.org/v2/everything?q=AAPL&from=2024-10-19&sortBy=popularity&apiKey=')

response = requests.get(url)

print(response.json())
