Feature: Search Artists feature

Search Discogs for specific artists

@SearchArtists
Scenario Outline: Search for artist
	Given I search for <artist>
	Then results for the <artist> are displayed
	Examples: 
	| artist  |
	| Blawan  |
	| Surgeon |
	| Objekt  |
