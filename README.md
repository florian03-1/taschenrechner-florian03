# taschenrechner-florian03
Ein Taschenrechner mit Formelparser - für die VBP-Challenge von @ElandaOfficial

#Funktionen
Der Taschenrechner enthält einen Formelparser, der mit dem Shunting-Yard Algorithmus einen eingegebenen Rechenausdruck von der Infix in die Postfix Notation umschreibt und dann berechnet. Das bedeutet, dass beim Eingeben eines Termes (wie z.B. 5*(3+4)*cos(8) alle mathematischen Rechenregeln beachtet werden (Klammern) und auch der cos korrekt berechnet wird.

Der Rechner bzw. Parser unterstützt folgende Funktionen:
- Adition (+)
- Subtraktion (-)
- Multiplikation (*)
- Division (/)
- Klammern
- Potenzierung (^)
- Wurzel (sqrt())
- Sinus (sin())
- Cosinus (cos())

Durch den abstrakten Aufabu des Parsers können leicht weitere Operatoren, wie der Tangens eingebaut werden.

Negative Zahlen werden auch unterstützt (einfach ein Minus vor die Zahl schreiben).
Gleitkommazahlen werden auch unterstützt.

#Umsetzung
Es wurde eine kleine GUI in WPF entwickelt die keinesfalls perfekt ist, aber für das Eingeben von Gleichungen reicht. Man kann entweder direkt ins Textfeld schreiben oder die Buttons benutzen. Wenn eine Rechnug ausgeführt wurde, wird sie der Historie-Listbox hinzugefügt.


Mehr Funktionen waren in der Zeit (2 Tage) nicht umsetzbar, bzw auch gar nicht verlangt.


#Screenshots

![image](https://user-images.githubusercontent.com/36850836/110341617-d0808f00-802a-11eb-8309-429b286fcff9.png)
