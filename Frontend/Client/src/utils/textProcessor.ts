export class TextProcessor {
  static highlightText(text: string, searchTerms: string[]): string {
    let highlightedText = text;
    
    searchTerms.forEach(term => {
      const regex = new RegExp(`(\\b${term.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')}\\b)`, 'gi');
      highlightedText = highlightedText.replace(regex, '<mark class="bg-yellow-200 text-yellow-900 px-1 rounded">$1</mark>');
    });

    return highlightedText;
  }

  static findNextOccurrence(text: string, term: string, startPosition: number = 0): number {
    const regex = new RegExp(`\\b${term.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')}\\b`, 'gi');
    const matches = Array.from(text.matchAll(regex));
    
    for (const match of matches) {
      if (match.index && match.index > startPosition) {
        return match.index;
      }
    }

    // If no match found after startPosition, return first occurrence
    return matches[0]?.index || -1;
  }

  static getAllOccurrences(text: string, term: string): number[] {
    const regex = new RegExp(`\\b${term.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')}\\b`, 'gi');
    const matches = Array.from(text.matchAll(regex));
    return matches.map(match => match.index || 0);
  }

  static scrollToElement(selector: string): void {
    const element = document.querySelector(selector);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }
  }
}