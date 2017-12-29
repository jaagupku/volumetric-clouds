# Volumetric clouds

## Esimese prototüüp

Eesmärk saavutada midagi sellist (Pilt raamatust GPU Pro 7):
![Eesmärk](https://raw.githubusercontent.com/jaagupku/volumetric-clouds/master/readmeimages/pic1.png)


Tulemus:
![Tulemus](https://raw.githubusercontent.com/jaagupku/volumetric-clouds/master/readmeimages/pic2.png)

## Asjad mida ei jõudnud

* Pilve kuju müra tekstuuriga on midagi viga. See ei muuda pilve kuju.
Täiesti valge ja hetkese `noiseShape` tekstuuriga on pilvede kuju sama.
Kui noiseShape tekstuuri paint.net'iga vaadata, siis `gba` kanalid paistavad õiged, 
kuid `r` kanal on kahtlane. Ning lisaks sellele kui GPUs kohe tekstuuri sampleda, 
siis see ei näe selline välja nagu paint.net'iga vaadates. `noiseErasion` tekstuur 
paistab olevat korrektne.
* Valguse samplemine koonusena. Hetkel teeb lihtsalt sirgjoones.
* Ilma tekstuur koos erinevate pilve tüüpide ja "tumedusega". Praegusel on ainult kattuvus.
