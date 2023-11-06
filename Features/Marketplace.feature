Feature: Marketplace feature

Browse the marketplace

@Marketplace
Scenario: Browse marketplace
	Given I navigate to the marketplace for format type 'Vinyl'
	When I filter items
	| Key        | Value          |
	| Ships From | United Kingdom |
	| Currency   | GBP            |
	| Genre      | Rock           |
	Then my results are filtered correctly
